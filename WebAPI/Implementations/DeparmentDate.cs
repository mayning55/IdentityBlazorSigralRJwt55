using System.Text.Json;
using System.Text.Json.Serialization;
using ClassLibrary.Settings;
using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Hubs;

namespace WebAPI.Implementations
{
    /// <summary>
    /// 部门数据的处理逻辑，IWebApiDateInterface的实现
    // EF的实现方式在PersonController
    /// </summary>
    public class DeparmentDate : IWebApiDateInterface<Department>
    {
        private readonly EFCoreDBContext dbContext;
        private readonly IHubContext<ChatHub> hubContext;
        public DeparmentDate(EFCoreDBContext dbContext, IHubContext<ChatHub> hubContext)
        {
            this.dbContext = dbContext;
            this.hubContext = hubContext;
        }
        public async Task<Responses> AddItem(Department item)
        {
            if (item is null)
            {
                return new Responses(false, "item is null???");

            }
            var dep = await dbContext.Departments.FirstOrDefaultAsync(d =>
                                                d.Name.ToLower().Equals(item.Name.ToLower()));
            if (dep == null)
            {
                Department newDep = new Department();
                {
                    newDep.Name = item.Name;
                }
                dbContext.Departments.Add(newDep);
                await dbContext.SaveChangesAsync();
                await hubContext.Clients.All.SendAsync("DepMessage", "Update");//变更后发送信息给在线用户重新加载List页面
                return new Responses(true, "done!");
            }
            return new Responses(false, "item exists");
        }
        public async Task<Responses> DeleteItem(long id)
        {
            if (id <= 0)
            {
                return new Responses(false, "id is not match");
            }
            var dep = await dbContext.Departments.Include(p => p.DepPersons)
                                .FirstOrDefaultAsync(d => d.Id == id);
            dbContext.Departments.Remove(dep);
            await dbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("DepMessage", "Update");
            return new Responses(true, "delete done");
        }

        public async Task<List<Department>> GetAll()
        {
            return await dbContext.Departments.ToListAsync();
        }
        /// <summary>
        /// .Include 和.ThenInclude存在Json maximum of 32 cycles问题
        /// 获取多对多关系的全部，返回Json格式化后的对象。也可以分开返回，如PersonController
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetById(long id)
        {
            var dep = dbContext.Departments.AsNoTracking()
                            .Include(p => p.DepPersons)
                            .ThenInclude(x => x.Person)
                            .FirstOrDefault(u => u.Id == id);
            var result = JsonSerializer.Serialize(dep, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            return result;

            // return dbContext.Departments.AsNoTracking()
            //                 .Include(p => p.DepPersons)
            //                 .ThenInclude(d => d.Person)
            //                 .FirstOrDefault(p => p.Id == id);
            //Json maximum of 32 cycles问题  //System.Text.Json          
            //var result = await (from dep in dbContext.Departments
            //                    select new Department
            //                    {
            //                        Id = dep.Id,
            //                        Name = dep.Name,
            //                        DepPersons = dep.DepPersons
            //                    }).FirstOrDefaultAsync(d => d.Id == id);
            //return result;
        }

        public async Task<IEnumerable<Department>> SearchByName(string name)
        {
            return await dbContext.Departments.Where(d => d.Name.Contains(name)).ToListAsync();
        }

        public async Task<Responses> EditItem(Department item)
        {
            if (item is null)
            {
                return new Responses(false, "item is null???");

            }
            var dep = await dbContext.Departments.FindAsync(item.Id);
            if (dep is not null)
            {
                dep.Name = item.Name;
                await dbContext.SaveChangesAsync();
                await hubContext.Clients.All.SendAsync("DepMessage", "Update");
                return new Responses(true, "done!");
            }
            return new Responses(false, "item is notfound");
        }
    }
}

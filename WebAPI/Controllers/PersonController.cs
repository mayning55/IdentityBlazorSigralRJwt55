using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DateClassLibrary.Data;
using ClassLibrary.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    /// <summary>
    /// EF的基本实现方式
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly EFCoreDBContext dbContext;
        private readonly IHubContext<ChatHub> hubContext;

        public PersonController(EFCoreDBContext dbContext, IHubContext<ChatHub> hubContext)
        {
            this.dbContext = dbContext;
            this.hubContext = hubContext;
        }

        // GET: Person/GetPersons
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await dbContext.Persons.ToListAsync();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsByPage()
        {
            return await dbContext.Persons.Take(5).ToListAsync();
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsByName(string? filterName, int currentPage)
        {
            if (string.IsNullOrWhiteSpace(filterName))
            {
                return await dbContext.Persons.Skip((currentPage - 1) * 5).Take(5).ToListAsync();
            }
            else
            {
                return await dbContext.Persons.Where(p => p.FirstName.Contains(filterName)).ToListAsync();
            }

        }
        // GET: Person/GetPersonByNumber
        [HttpGet]
        public async Task<ActionResult<Person>> GetPersonById(long id)
        {
            var person = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }
        // POST: Person/AddPerson
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]

        public async Task<ActionResult> AddPerson(Person person)
        {
            var item = new Person
            {
                Number = person.Number,
                FirstName = person.FirstName,
            };
            dbContext.Persons.Add(item);
            await dbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("NoteMessage", "Update");//变更后发送信息给在线用户重新加载List页面，下同。
            return CreatedAtAction(nameof(GetPersonById), new { Id = person.Id }, person);
        }

        // DELETE: Person/DeletePerson
        [HttpDelete]
        [Authorize(Roles = "AdminRole")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            dbContext.Persons.Remove(person);
            await dbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("NoteMessage", "Update");
            return NoContent();
        }

        // PUT:Person/EditPerson
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Person>> EditPerson(long id, Person person)
        {
            var newPerson = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == id);

            if (id != newPerson.Id)
            {
                return BadRequest();
            }
            newPerson.Number = person.Number;
            newPerson.FirstName = person.FirstName;
            dbContext.Persons.Update(newPerson);

            try
            {
                await dbContext.SaveChangesAsync();
                await hubContext.Clients.All.SendAsync("NoteMessage", "Update");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        /// <summary>
        /// Person与Department分开读取，一次获取全部信息会出现inclub,theninclub的Json maximum of 32 cycles问题。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<DepSelect>> GetDepSelectAsync(long id)
        {
            List<DepSelect> depSelectList = new List<DepSelect>();
            var allDep = await dbContext.Departments.ToListAsync();
            if (id == 0)//用于新增，
            {
                foreach (var dep in allDep)
                {
                    depSelectList.Add(new DepSelect
                    {
                        Id = dep.Id,
                        DepartmentName = dep.Name,
                        IsSelect = false
                    });

                }
                return Ok(depSelectList);
            }
            else
            {
                Person person = await dbContext.Persons.Include(x => x.DepPersons).FirstOrDefaultAsync(p => p.Id == id);
                var personDep = new HashSet<long>(person.DepPersons.Select(p => p.DepartmentId));
                foreach (var dep in allDep)
                {
                    depSelectList.Add(new DepSelect
                    {
                        Id = dep.Id,
                        DepartmentName = dep.Name,
                        IsSelect = personDep.Contains(dep.Id)
                    });
                }
                return Ok(depSelectList);

            }
        }
        /// <summary>
        /// 将选择好的部门Id，更新DepPersons的两者关系。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectDep"></param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateDepSelectAsync(long id, long[] selectDep)
        {
            Person person = await dbContext.Persons.Include(x => x.DepPersons).FirstOrDefaultAsync(p => p.Id == id);
            if (selectDep != null)
            {
                var selectDepN = new HashSet<long>(selectDep);
                var personDep = new HashSet<long>(person.DepPersons.Select(p => p.DepartmentId));
                foreach (var dep in dbContext.Departments)
                {
                    if (selectDepN.Contains(dep.Id))
                    {
                        if (!personDep.Contains(dep.Id))
                        {
                            person.DepPersons.Add(new DepPerson { Department = dep, Person = person });
                        }
                    }
                    else
                    {
                        if (personDep.Contains(dep.Id))
                        {
                            var depRemove = person.DepPersons.FirstOrDefault(
                                                            c => c.DepartmentId == dep.Id);
                            person.DepPersons.Remove(depRemove);
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            return Ok();
        }

        private bool PersonExists(long id)
        {
            return dbContext.Persons.Any(e => e.Id == id);
        }
        public List<DepSelect> DepSelectList;
    }
}

using ClassLibrary.Settings;
using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Implementations;

public class OrganizationData : IWebApiDataInterface<Organization>
{
    private readonly EFCoreDBContext dbContext;
    public OrganizationData(EFCoreDBContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<Responses> AddItem(Organization item)
    {
        if (item is null)
        {
            return new Responses(false, "item is null???");
        }
        var org = await dbContext.Organizations.FirstOrDefaultAsync(x => x.Id == item.Id);
        if (org == null)
        {
            dbContext.Organizations.Add(item);
            await dbContext.SaveChangesAsync();
            return new Responses(true, "Item Add!");
        }
        return new Responses(false, "Item Exists!");
    }

    public async Task<Responses> DeleteItem(long id)
    {
        if (id <= 0)
        {
            return new Responses(false, "item is null???");
        }
        var org = await dbContext.Organizations.Include(a=>a.OrganizationSub)
                                        .ThenInclude(b=>b.OrganizationSub)
                                        .ThenInclude(c=>c.OrganizationSub)
                                        .FirstOrDefaultAsync(x => x.Id == id);
        if (org == null)
        {
            return new Responses(false, "item not found!");
        }
        dbContext.Organizations.Remove(org);
        await dbContext.SaveChangesAsync();
        return new Responses(true, "Item has deleted");
    }

    public async Task<Responses> EditItem(Organization item)
    {
        if (item is null)
        {
            return new Responses(false, "item is null???");

        }
        var org = await dbContext.Organizations.FindAsync(item.Id);
        if (org is not null)
        {
            org.Name = item.Name;
            org.PorgId = item.PorgId;
            await dbContext.SaveChangesAsync();            
            return new Responses(true, "Item Update!");
        }
        return new Responses(false, "item  notfound");
    }

    public Task<string> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Organization> GetItemById(long id)
    {
        return await dbContext.Organizations.Include(a => a.OrganizationSub)
                                                .ThenInclude(b => b.OrganizationSub)
                                                .ThenInclude(c=>c.OrganizationSub)
                                                .OrderBy(a => a.Id).FirstAsync();
    }

    public async Task<List<Organization>> GetItems()
    {
        return await dbContext.Organizations.Select(a=> new Organization{
            Id = a.Id,
            Name = a.Name,
        }).ToListAsync();
    }

    public Task<IEnumerable<Organization>> SearchByName(string name)
    {
        throw new NotImplementedException();
    }
}

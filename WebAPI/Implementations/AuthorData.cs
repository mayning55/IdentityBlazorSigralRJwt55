using ClassLibrary.Settings;
using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Hubs;

namespace WebAPI.Implementations;

public class AuthorData : IWebApiDataInterface<Author>
{
    private readonly EFCoreDBContext dbContext;
    private readonly IHubContext<ChatHub> hubContext;
    public AuthorData(EFCoreDBContext dbContext, IHubContext<ChatHub> hubContext)
    {
        this.dbContext = dbContext;
        this.hubContext = hubContext;
    }
    public async Task<Responses> AddItem(Author item)
    {
        if (item is null)
        {
            return new Responses(false, "item is null???");
        }
        var author = await dbContext.Authors.FirstOrDefaultAsync(x => x.Id == item.Id);
        if (author == null)
        {
            dbContext.Authors.Add(item);
            await dbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("BookMessage", "Update");
            return new Responses(true, "Author Add!");
        }
        return new Responses(false, "Item Exists!");
    }

    public async Task<Responses> DeleteItem(long id)
    {
        if (id <= 0)
        {
            return new Responses(false, "item is null???");
        }
        var author = await dbContext.Authors.FirstOrDefaultAsync(x => x.Id == id);
        if (author == null)
        {
            return new Responses(false, "item not found!");
        }
        var books = await dbContext.Books.Where(x => x.AuthorId == author.Id).ToListAsync();
        foreach (var item in books)
        {
            item.AuthorId = null;
        }
        dbContext.Authors.Remove(author);
        await dbContext.SaveChangesAsync();
        await hubContext.Clients.All.SendAsync("BookMessage", "Update");
        return new Responses(true, "Item has deleted");
    }

    public async Task<Responses> EditItem(Author item)
    {
        if (item is null)
        {
            return new Responses(false, "item is null???");

        }
        var author = await dbContext.Authors.FindAsync(item.Id);
        if (author is not null)
        {
            author.AuthorName = item.AuthorName;
            await dbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("BookMessage", "Update");
            return new Responses(true, "Item Update!");
        }
        return new Responses(false, "item  notfound");
    }

    public Task<string> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Author> GetItemById(long id)
    {
        return await dbContext.Authors.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Author>> GetItems()
    {
        return await dbContext.Authors.ToListAsync();
    }

    public Task<IEnumerable<Author>> SearchByName(string name)
    {
        throw new NotImplementedException();
    }
}


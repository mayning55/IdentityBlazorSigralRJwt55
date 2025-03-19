using ClassLibrary.Settings;
using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Hubs;

namespace WebAPI.Implementations;

public class BookData : IWebApiDataInterface<Book>
{
    private readonly EFCoreDBContext dbContext;
    private readonly IHubContext<ChatHub> hubContext;
    public BookData(EFCoreDBContext dbContext, IHubContext<ChatHub> hubContext)
    {
        this.dbContext = dbContext;
        this.hubContext = hubContext;
    }
    public async Task<Responses> AddItem(Book item)
    {
        if (item is null)
        {
            return new Responses(false, "item is null???");
        }
        var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == item.Id);
        if (book == null)
        {
            dbContext.Books.Add(item);
            await dbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("BookMessage", "Update");
            return new Responses(true, "Book Add!");
        }
        return new Responses(false, "Item Exists!");
    }

    public async Task<Responses> DeleteItem(long id)
    {
        if (id <= 0)
        {
            return new Responses(false, "item is null???");
        }
        var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
        if (book == null)
        {
            return new Responses(false, "item not found!");
        }
        dbContext.Books.Remove(book);
        await dbContext.SaveChangesAsync();
        await hubContext.Clients.All.SendAsync("BookMessage", "Update");
        return new Responses(true, "Item has deleted");
    }

    public async Task<Responses> EditItem(Book item)
    {
        if (item is null)
        {
            return new Responses(false, "item is null???");

        }
        var book = await dbContext.Books.FindAsync(item.Id);
        if (book is not null)
        {
            book.Title = item.Title;
            book.AuthorId = item.AuthorId;
            book.PublicationDate = item.PublicationDate;
            book.Price = item.Price;
            //dbContext.Update(book);
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

    public async Task<Book> GetItemById(long id)
    {
        return await dbContext.Books.Include(a => a.Author).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Book>> GetItems()
    {
        return await dbContext.Books.Include(a => a.Author).ToListAsync();

    }

    public Task<IEnumerable<Book>> SearchByName(string name)
    {
        throw new NotImplementedException();
    }
    

}


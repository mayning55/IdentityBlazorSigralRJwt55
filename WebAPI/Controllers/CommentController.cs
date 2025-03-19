using ClassLibrary.Settings;
using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly EFCoreDBContext dbContext;
        public CommentController(EFCoreDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<List<Comment>> GetCommentsAsync(long id)
        {
            return await dbContext.Comments.Where(c => c.BookId == id).ToListAsync();
        }
        [HttpPost]

        public async Task<Responses> AddComment(Comment item)
        {
            if (item is null)
            {
                return new Responses(false, "item is null???");
            }
            else
            {
                dbContext.Comments.Add(item);
                await dbContext.SaveChangesAsync();
                //
                return new Responses(true, "Commnet Add!");
            }
        }
        [HttpDelete]
        public async Task<Responses> DelComment(long id)
        {
            var comment = dbContext.Comments.FirstOrDefault(c => c.Id == id);
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
            return new Responses(true, "Comment delete!");
        }
    }
}

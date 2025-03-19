using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BookController(IWebApiDataInterface<Book> authInterface) : DateController<Book>(authInterface)
    {

    }
}

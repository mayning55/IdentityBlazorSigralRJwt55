using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthorController(IWebApiDataInterface<Author> authInterface) : DateController<Author>(authInterface)
    {
    }
}

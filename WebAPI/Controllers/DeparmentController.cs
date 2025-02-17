using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DeparmentController(IWebApiDateInterface<Department> dateInterface)
        : DateController<Department>(dateInterface)
    {
    }
}

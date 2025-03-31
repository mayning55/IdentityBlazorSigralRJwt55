using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AdminRole")]
    public class OrganizationController(IWebApiDataInterface<Organization> dataInterface) : DateController<Organization>(dataInterface)
    {
    }
}

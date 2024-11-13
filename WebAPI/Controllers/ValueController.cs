using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "AdminRole")]
        public string AdminValue()
        {
            var name = this.User.FindFirst(ClaimTypes.Name);
            var role = this.User.FindFirst(ClaimTypes.Role);
            return name.Value + " "+ role.Value + "ok";
        }
        [HttpGet]

        [Authorize(Roles = "NormalUser")]
        public string UserValue()
        {
            var name = this.User.FindFirst(ClaimTypes.Name);
            var role = this.User.FindFirst(ClaimTypes.Role);
            return name.Value + " " + role.Value + "ok";
        }
        [HttpGet]
        [Authorize(Roles = "AdminRole,NormalUser")]
        public string MulitValue()
        {
            var result = this.User.FindFirst(ClaimTypes.Name);
            var role = this.User.FindFirst(ClaimTypes.Role);
            return result.Value + " " + role.Value + "ok";
        }

        [HttpGet]
        [AllowAnonymous]
        public string AllowAnonymous()
        {
            return "AllowAnonymous";
        }
    }
}

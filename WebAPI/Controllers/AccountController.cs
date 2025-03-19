using ClassLibrary.Settings;
using LoginClassLibrary;
using LoginClassLibrary.Account;
using LoginClassLibrary.Login;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUser user;

        public AccountController(IUser user)
        {
            this.user = user;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var result = await user.LoginAsync(loginRequest);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync(RefreshToken tokenHash)
        {
            if (tokenHash == null)
            {
                return BadRequest("token is null");
            }
            var result = await user.RefreshTokenAsync(tokenHash);
            return Ok(result);
        }
    }
}

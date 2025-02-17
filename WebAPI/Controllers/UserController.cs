using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DateClassLibrary.Data;
using ClassLibrary.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly EFCoreDBContext dbContext;
        private readonly UserManager<UserExtend> userManager;

        public UserController(EFCoreDBContext dbContext, UserManager<UserExtend> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        /// <summary>
        /// 返回UserInfo指定数据。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            IEnumerable<UserInfo> user = users.Select(p =>
                                        new UserInfo
                                        {
                                            Id = p.Id,
                                            UserName = p.UserName,
                                            IsDisabled = p.IsDisabled,
                                            CreateDatetime = p.CreateDatetime,
                                            Position = p.Position,
                                            Email = p.Email
                                        });
            return Ok(user);
        }
        [HttpGet]
        public async Task<ActionResult<UserInfo>> GetUserByNameAsync(string userName)
        {
            if (await userManager.FindByNameAsync(userName) == null)
            {
                return NotFound();
            }
            else
            {
                var user = await userManager.FindByNameAsync(userName);
                UserInfo userInfos = new UserInfo()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    IsDisabled = user.IsDisabled,
                    CreateDatetime = user.CreateDatetime,
                    Position = user.Position,
                    Email = user.Email
                };
                return Ok(userInfos);
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditUserAsync(string userID, UserInfo userInfo)
        {
            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.UserName = userInfo.UserName;
                user.Position = userInfo.Position;
                user.Email = userInfo.Email;
                var result = await userManager.UpdateAsync(user);
                return Ok(result);
            }
        }
        [HttpPost]
        public async Task<ActionResult<UserExtend>> AddUserAsync(CreateUser createUser)
        {
            if (await userManager.FindByNameAsync(createUser.UserName) == null)
            {
                var user = new UserExtend
                {
                    UserName = createUser.UserName,
                    Position = createUser.Position,
                    CreateDatetime = DateTime.UtcNow.AddHours(8)
                };
                await userManager.CreateAsync(user, createUser.Password);
            }
            return NotFound();
        }
        [HttpPut]
        public async Task<IActionResult> DisableUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.IsDisabled = true;
                await userManager.UpdateAsync(user);
                await userManager.SetLockoutEnabledAsync(user, true);
                await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                return NoContent();
            }
        }
        [HttpPut]
        public async Task<IActionResult> EnableUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.IsDisabled = false;
                await userManager.UpdateAsync(user);
                await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(8));
                return NoContent();
            }
        }
        [HttpPost]
        public async Task<IActionResult> RestUserPasswordAsync(string id, UserPassword userPassword)
        {
            if (userPassword is null || userPassword.Password == "string")
            {
                return NotFound("string");//todo
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var result = await userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, userPassword.Password);
                }
            }
            return NoContent();
        }

    }
}

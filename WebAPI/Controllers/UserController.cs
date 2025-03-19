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
    [Authorize(Roles = "AdminRole")]
    public class UserController : ControllerBase
    {
        private readonly EFCoreDBContext dbContext;
        private readonly UserManager<UserExtend> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(EFCoreDBContext dbContext, UserManager<UserExtend> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
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
                                            //Id = p.Id,
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
                    //Id = user.Id,
                    UserName = user.UserName,
                    IsDisabled = user.IsDisabled,
                    CreateDatetime = user.CreateDatetime,
                    Position = user.Position,
                    Email = user.Email
                };
                return Ok(userInfos);
            }
        }
        [HttpPost]
        public async Task<ActionResult<CreateUser>> AddUserAsync(CreateUser createUser)
        {
            if (await userManager.FindByNameAsync(createUser.UserName) == null)
            {
                var user = new UserExtend
                {
                    UserName = createUser.UserName,
                    Position = createUser.Position,
                    Email = createUser.Email,
                    CreateDatetime = DateTime.UtcNow.AddHours(8)
                };
                await userManager.CreateAsync(user, createUser.Password);
                return Ok(createUser.UserName);
            }
            else
            {
                return NotFound("user is exists");
            }

        }
        [HttpPut]
        public async Task<IActionResult> EditUserAsync(string userName, UserInfo userInfo)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                //user.UserName = userInfo.UserName;
                user.Position = userInfo.Position;
                user.Email = userInfo.Email;
                var result = await userManager.UpdateAsync(user);
                return Ok(result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> DisableUserAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
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
                return Ok();
            }
        }
        [HttpPut]
        public async Task<IActionResult> EnableUserAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.IsDisabled = false;
                await userManager.UpdateAsync(user);
                await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(8));
                return Ok();
            }
        }
        [HttpPost]
        public async Task<IActionResult> RestUserPasswordAsync(string userName, UserPassword userPassword)
        {
            if (userPassword is null || userPassword.Password == "string")
            {
                return NotFound();//todo
            }
            var user = await userManager.FindByNameAsync(userName);
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
                return Ok();
            }
        }
        [HttpGet]
        public async Task<ActionResult<UserInRole>> GetRoleOwnUserAsync(string userName)
        {
            if (userName is null)
            {
                return NotFound();
            }
            var user = await userManager.FindByNameAsync(userName);
            if (user != null)
            {
                List<UserInRole> roleOwnUsers = new List<UserInRole>();
                var allRole = await roleManager.Roles.ToListAsync();
                if (userName is null)
                {
                    foreach (var role in allRole)
                    {
                        roleOwnUsers.Add(new UserInRole
                        {
                            Name = role.Name,
                            IsSelect = false
                        });
                    }
                    return Ok(roleOwnUsers);
                }
                else
                {
                    foreach (var role in allRole)
                    {
                        if (await userManager.IsInRoleAsync(user, role.Name))
                        {
                            roleOwnUsers.Add(new UserInRole
                            {
                                Name = role.Name,
                                IsSelect = true
                            });
                        }
                        else
                        {
                            roleOwnUsers.Add(new UserInRole
                            {
                                Name = role.Name,
                                IsSelect = false
                            });
                        }
                    }
                    return Ok(roleOwnUsers);
                }
            }
            return NotFound();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateRoleOwnUserAsync(string userName, List<UserInRole> roleOwnUsers)
        {
            if (userName is null)
            {
                return NotFound();
            }
            var user = await userManager.FindByNameAsync(userName);
            if (user != null)
            {
                for (int i = 0; i < roleOwnUsers.Count(); i++)
                {
                    var role = await roleManager.FindByNameAsync(roleOwnUsers[i].Name);
                    if (roleOwnUsers[i].IsSelect)
                    {
                        await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!roleOwnUsers[i].IsSelect && (await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                }
                return Ok();
            }
            return NotFound();
        }

    }
}

using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AdminRole")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<UserExtend> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<UserExtend> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<List<string>> GetRolesAsync()
        {
            var roles = await roleManager.Roles.Select(r => r.Name).ToListAsync();
            return roles;
        }
        /// <summary>
        /// 使用自定义响应，返回信息。UserController则没有。
        /// 参阅：https://learn.microsoft.com/zh-cn/aspnet/core/web-api/action-return-types?view=aspnetcore-8.0
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Responses> AddRoleAsync(string roleName)
        {
            if (roleName is null)
            {
                return new Responses(false, "rolename is null???");
            }
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                IdentityRole newRole = new IdentityRole { Name = roleName };
                await roleManager.CreateAsync(newRole);
                return new Responses(true, "rolename is add");
            }
            return new Responses(false, "rolename is extitx");
        }
        [HttpPut]
        public async Task<Responses> EditRoleAsync(string roleName, string newRoleName)
        {
            var newrole = await roleManager.FindByNameAsync(newRoleName);
            if (newrole != null)
            {
                return new Responses(false, "rolename is extitx???");
            }
            var editrole = await roleManager.FindByNameAsync(roleName);
            if (editrole != null)
            {
                editrole.Name = newRoleName;
                await roleManager.UpdateAsync(editrole);
                return new Responses(true, "rolename is update");
            }
            return new Responses(false, "rolename is null???");
        }

        [HttpGet]
        public async Task<ActionResult<UserInRole>> GetUserInRoleAsync(string roleName)
        {
            if (roleName is null)
            {
                return NotFound();
            }
            var role = await roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                List<UserInRole> userInRoles = new List<UserInRole>();
                var allUser = await userManager.Users.ToListAsync();
                if (roleName is null)
                {
                    foreach (var user in allUser)
                    {
                        userInRoles.Add(new UserInRole
                        {
                            Name = user.UserName,
                            IsSelect = false
                        });
                    }
                    return Ok(userInRoles);
                }
                else
                {
                    foreach (var user in allUser)
                    {
                        if (await userManager.IsInRoleAsync(user, roleName))
                        {
                            userInRoles.Add(new UserInRole
                            {
                                Name = user.UserName,
                                IsSelect = true
                            });
                        }
                        else
                        {
                            userInRoles.Add(new UserInRole
                            {
                                Name = user.UserName,
                                IsSelect = false
                            });
                        }
                    }
                    return Ok(userInRoles);
                }
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut]
        public async Task<Responses> UpdateUserInRoleAsync(string roleName, List<UserInRole> userInRoles)
        {
            if (roleName is null)
            {
                return new Responses(false, "Item not found!");
            }
            var role = await roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                for (int i = 0; i < userInRoles.Count(); i++)
                {
                    var user = await userManager.FindByNameAsync(userInRoles[i].Name);
                    if (userInRoles[i].IsSelect)
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                    else if (!userInRoles[i].IsSelect && (await userManager.IsInRoleAsync(user, roleName)))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                    else
                    {
                        continue;
                    }
                }
                return new Responses(true, "ItemDetail Update Success!");
            }
            return new Responses(false, "Item not found!");
        }
    }
}

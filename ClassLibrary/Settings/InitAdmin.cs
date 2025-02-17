using DateClassLibrary.Data;
using Microsoft.AspNetCore.Identity;

namespace ClassLibrary.Settings
{
    /// <summary>
    /// 初始化Admin用户，初始化完成后可删除。
    /// 还可以使用Data-Seed,参阅：https://learn.microsoft.com/zh-cn/ef/core/modeling/data-seeding
    /// </summary>
    public class InitAdmin
    {
        private readonly UserManager<UserExtend> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public InitAdmin(UserManager<UserExtend> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task Manage()
        {
            if (await roleManager.RoleExistsAsync("AdminRole") == false)
            {
                IdentityRole adminRole = new IdentityRole();
                adminRole.Name = "AdminRole";
                var result = await roleManager.CreateAsync(adminRole);
            }
            if (await roleManager.RoleExistsAsync("NormalUser") == false)
            {
                IdentityRole normalUserRole = new IdentityRole();
                normalUserRole.Name = "NormalUser";
                var result = await roleManager.CreateAsync(normalUserRole);
            }
            var userAdmin = await userManager.FindByNameAsync("Admin");
            if (userAdmin == null)
            {
                userAdmin = new UserExtend()
                {
                    UserName = "Admin"
                };
                var result = await userManager.CreateAsync(userAdmin, "123456");
            }
            if (!await userManager.IsInRoleAsync(userAdmin, "AdminRole"))
            {
                var result = await userManager.AddToRoleAsync(userAdmin, "AdminRole");
            }
            if (!await userManager.IsInRoleAsync(userAdmin, "NormalUser"))
            {
                var result = await userManager.AddToRoleAsync(userAdmin, "NormalUser");
            }
            var user1 = await userManager.FindByNameAsync("User1");
            if (user1 == null)
            {
                user1 = new UserExtend()
                {
                    UserName = "User1"
                };
                var result = await userManager.CreateAsync(user1, "123456");
            }
            if (!await userManager.IsInRoleAsync(user1, "NormalUser"))
            {
                var result = await userManager.AddToRoleAsync(user1, "NormalUser");
            }
        }
    }
}

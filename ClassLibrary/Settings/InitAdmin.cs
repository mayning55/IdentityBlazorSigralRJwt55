using ClassLibrary.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Settings
{
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
            var user1= await userManager.FindByNameAsync("User1");
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

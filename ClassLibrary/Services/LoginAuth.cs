using ClassLibrary.Data;
using LoginClassLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClassLibrary.Services
{
    internal class LoginAuth : IUser
    {
        private readonly UserManager<UserExtend> userManager;
        private readonly IConfiguration configuration;

        public LoginAuth(UserManager<UserExtend> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<LoginResponse> LoginrUserAsync(LoginRequest loginRequest)
        {
            var user = await userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null)
            {
                return new LoginResponse(false, "User not Found");
            }
            var result = await userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (result == false)
            {
                return new LoginResponse(false, "Invalid credntials");
            }
            else
            {
                var roleList = new List<string>();
                foreach (var role in await userManager.GetRolesAsync(user))
                {
                    roleList.Add(role);
                }
                return new LoginResponse(true, "Login successfully", CreateToken(user,roleList));
            }
        }
        private string CreateToken(UserExtend user,List<string> rolename)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]!));
            var credetials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            foreach (var r in rolename)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, r.ToString()));
            }
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddSeconds(Convert.ToDouble(configuration["JWT:ExpireSeconds"])),
                signingCredentials: credetials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

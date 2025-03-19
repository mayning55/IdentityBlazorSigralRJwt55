using DateClassLibrary.Data;
using ClassLibrary.Settings;
using LoginClassLibrary;
using LoginClassLibrary.Account;
using LoginClassLibrary.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Implementations
{
    /// <summary>
    /// 用户登录验证的处理逻辑，即如何实现IUser接口的动作
    //返回带用户名和角色及相关信息的Token。
    /// </summary>
    public class LoginAuth : IUser
    {
        private readonly UserManager<UserExtend> userManager;
        private readonly IConfiguration configuration;
        private readonly EFCoreDBContext dbContext;
        public LoginAuth(UserManager<UserExtend> userManager, IConfiguration configuration, EFCoreDBContext dbContext)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.dbContext = dbContext;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null)
            {
                return new LoginResponse(false, "User not Found");
            }
            var result = await userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (result == false)
            {
                return new LoginResponse(false, "Invalid Login");
            }
            else
            {
                //user.JWTVer++;
                //await userManager.UpdateAsync(user);
                var roleList = new List<string>();
                foreach (var role in await userManager.GetRolesAsync(user))
                {
                    roleList.Add(role);
                }
                string jwtToken = CreateToken(user, roleList);
                string refreshToken = GenerateRefreshToken();
                var tokenInfo = await dbContext.RefreshTokenInfos.FirstOrDefaultAsync(r => r.UserName == user.UserName);
                if (tokenInfo is not null)
                {
                    tokenInfo.Token = refreshToken;
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    await AddToDatabase(new RefreshTokenInfo()
                    {
                        Token = refreshToken,
                        UserName = user.UserName,
                    });
                    //await dbContext.RefreshTokenInfos.AddRangeAsync(
                    //    new RefreshTokenInfo
                    //    {
                    //        Token = refreshToken,
                    //        UserName = user.UserName,
                    //    });
                }
                return new LoginResponse(true, "Login Successfully", jwtToken, refreshToken);
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken tokenhash)
        {
            if (tokenhash == null)
            {
                return new LoginResponse(false, "null!!!!");
            }
            var findToke = await dbContext.RefreshTokenInfos.FirstOrDefaultAsync(t => t.Token!.Equals(tokenhash.Token));
            if (findToke == null)
            {
                return new LoginResponse(false, "Token not Found");
            }
            var user = await userManager.FindByNameAsync(findToke.UserName);
            if (user == null)
            {
                return new LoginResponse(false, "User not Found");
            }
            var roleList = new List<string>();
            foreach (var role in await userManager.GetRolesAsync(user))
            {
                roleList.Add(role);
            }
            string jwtToken = CreateToken(user, roleList);
            string refreshToken = GenerateRefreshToken();

            var updateRefreshToken = await dbContext.RefreshTokenInfos.FirstOrDefaultAsync(t => t.UserName == user.UserName);
            if (updateRefreshToken == null)
            {
                return new LoginResponse(false, "user not foud,no token");
            }

            updateRefreshToken.Token = refreshToken;
            await dbContext.SaveChangesAsync();
            return new LoginResponse(true, "Token Refresh!", jwtToken, refreshToken);
        }

        /// <summary>
        /// 用户登录成功生成Token
        /// </summary>
        /// <param name="user"></param>登录的用户
        /// <param name="rolename"></param>登录用户的角色列表
        /// <returns></returns>
        private string CreateToken(UserExtend user, List<string> rolename)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]!));
            var credetials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            foreach (var r in rolename)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, r.ToString()));
            }
            //userClaims.Add(new Claim("JWTVer", user.JWTVer.ToString()));
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: userClaims,
                //expires: DateTime.Now.AddSeconds(Convert.ToDouble(configuration["JWT:ExpireSeconds"])),
                expires: DateTime.Now.AddMinutes(10),//调小一点，测试过期时间是否刷新Token.
                signingCredentials: credetials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 生成随机值，验证Token的更新。
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = dbContext.Add(model!);
            await dbContext.SaveChangesAsync();
            return (T)result.Entity;
        }
    }
}

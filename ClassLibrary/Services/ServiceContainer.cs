using DateClassLibrary.Data;
using ClassLibrary.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ClassLibrary.Services
{
    /// <summary>
    /// 通过ConfigureServices 容器方法配置各服务，包含：数据库连接，Jwt，Identity用户登录。
    /// </summary>
    public static class ServiceContainer
    {
        public static IServiceCollection DIServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            //db连接
            services.AddDbContext<EFCoreDBContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("SQLServerConnection"),
                b => b.MigrationsAssembly(typeof(ServiceContainer).Assembly.FullName)),
                    ServiceLifetime.Scoped);
            //jwt验证配置
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(0),//时钟偏移，过期时间=偏移时间+有效时间
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]!))
                };
            });
            //Identity配置
            IdentityBuilder identityBuilder = new IdentityBuilder(typeof(UserExtend), typeof(IdentityRole), services);
            identityBuilder.AddEntityFrameworkStores<EFCoreDBContext>()
                    .AddDefaultTokenProviders()
                    .AddUserManager<UserManager<UserExtend>>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddSignInManager();
            services.AddIdentityCore<UserExtend>(options =>
            {
                options.Password.RequireDigit = true;//必须包含数字
                options.Password.RequireLowercase = false;//必须包含小写字母
                options.Password.RequireNonAlphanumeric = false;//必须包含非字母数字
                options.Password.RequireUppercase = false;//必须大写字母
                options.Password.RequiredLength = 3;//长度
                options.Password.RequiredUniqueChars = 3;//可重复唯一
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultAuthenticatorProvider;//根据Token重置密码
            });
            return services;
        }
    }
}
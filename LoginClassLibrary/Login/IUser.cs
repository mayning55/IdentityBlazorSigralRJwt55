using LoginClassLibrary.Account;

namespace LoginClassLibrary.Login
{
    /// <summary>
    /// 与IWebApiDateInterface一样，接口实现依赖关系抽象化。用于用户登录。
    // 用户和角色管理则是使用EF的方式。todo！！！！！
    /// </summary>
    public interface IUser
    {
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken tokenHash);
    }
}

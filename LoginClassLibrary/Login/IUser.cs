using LoginClassLibrary.Account;

namespace LoginClassLibrary.Login
{
    /// <summary>
    /// 用户登录和刷新Token
    /// </summary>
    public interface IUser
    {
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken tokenHash);
    }
}

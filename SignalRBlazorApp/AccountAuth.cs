using LoginClassLibrary;
using LoginClassLibrary.Account;
using LoginClassLibrary.Login;
using SignalRBlazorApp.Services;
using System.Net.Http.Json;

namespace SignalRBlazorApp
{
    /// <summary>
    /// 用户登录验证的处理逻辑，即如何实现IUser接口的动作
    /// </summary>
    /// <param name="getHttpClient"></param> ：重构后的HttpClient
    public class AccountAuth(GetHttpClient getHttpClient) : IUser
    {
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync("Account/Login", loginRequest);
            if (!result.IsSuccessStatusCode)
            {
                return new LoginResponse(false, "error!");
            }
            return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken tokenHash)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync("Account/RefreshToken", tokenHash);
            if (!result.IsSuccessStatusCode)
            {
                return new LoginResponse(false, "error!");
            }
            return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
        }
    }
}

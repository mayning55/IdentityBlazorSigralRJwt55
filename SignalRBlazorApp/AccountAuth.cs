using LoginClassLibrary;
using LoginClassLibrary.Account;
using LoginClassLibrary.Login;
using SignalRBlazorApp.Services;
using System.Net.Http.Json;

namespace SignalRBlazorApp
{
    /// <summary>
    /// 用户登录验证
    /// </summary>
    /// <param name="getHttpClient"></param>
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

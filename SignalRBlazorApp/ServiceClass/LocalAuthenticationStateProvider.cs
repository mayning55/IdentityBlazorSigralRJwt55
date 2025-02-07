using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using SignalRBlazorApp.Services;
using LoginClassLibrary.Account;

namespace SignalRBlazorApp.Services
{
    /// <summary>
    /// 用户登录状态，访问需要授权的页面时带上保存在本地的Token。
    /// </summary>
    public class LocalAuthenticationStateProvider(ILocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private const string LocalStorageKey = "auth";
        private readonly ClaimsPrincipal anonymous = new(new ClaimsPrincipal());//未登录

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorageService.GetItemAsStringAsync(LocalStorageKey);
            if (string.IsNullOrEmpty(token))
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(token);
            if (deserializeToken == null)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
            var name = GetClaims(deserializeToken.Token);
            if (string.IsNullOrEmpty(name))
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
            var clarims = SetClaimPrincipal(name);
            if (clarims is null)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
            else
            {
                return await Task.FromResult(new AuthenticationState(clarims));
            }
        }

        public static ClaimsPrincipal SetClaimPrincipal(string name)
        {
            if (name is null)
            {
                return new ClaimsPrincipal();
            }
            else
            {
                return new ClaimsPrincipal(new ClaimsIdentity(
                    [
                        new(ClaimTypes.Name, name!),
                    ], "JwtAuth"));
            }
        }

        private static string GetClaims(string JwtToken)
        {
            if (string.IsNullOrEmpty(JwtToken))
            {
                return null!;
            }
            var handler = new JsonWebTokenHandler();
            var token = handler.ReadJsonWebToken(JwtToken);
            var name = token.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name)!.Value;
            return name;
        }
        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimPrincipal = new ClaimsPrincipal();
            if (userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializeSession = Serializations.SerializeOjb(userSession);
                await localStorageService.SetItemAsStringAsync(LocalStorageKey,serializeSession);
                var getUserClaims = GetClaims(userSession.Token!);
                claimPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                await localStorageService.RemoveItemAsync(LocalStorageKey);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimPrincipal)));

        }


    }
}

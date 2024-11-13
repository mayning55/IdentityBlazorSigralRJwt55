using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace SignalRBlazorApp
{
    public class LocalAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string LocalStorageKey = "auth";
        private readonly ILocalStorageService localStorageService;

        public LocalAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        private readonly ClaimsPrincipal anonymous = new(new ClaimsPrincipal());//未登录

        public override async  Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string toke = await localStorageService.GetItemAsStringAsync(LocalStorageKey)!;
            if (string.IsNullOrEmpty(toke))
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
            var name = GetClaims(toke);
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
                return (null!);
            }
            //var handler = new JwtSecurityTokenHandler();
            //var token = handler.ReadJwtToken(JwtToken);
            var handler = new JsonWebTokenHandler();
            var token = handler.ReadJsonWebToken(JwtToken);

            var name = token.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name)!.Value;

            return (name);
        }
        public async Task UpdateauthenticationState(string jwtToken)
        {
            var claims = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                var name = GetClaims(jwtToken);
                if (string.IsNullOrEmpty(name))
                    return;

                var setClaims = SetClaimPrincipal(name);
                if (setClaims is null)
                    return;
                await localStorageService.SetItemAsStringAsync(LocalStorageKey, jwtToken);
            }
            else
            {
                await localStorageService.RemoveItemAsync(LocalStorageKey);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));

        }


    }
}

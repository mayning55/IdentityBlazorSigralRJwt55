using Blazored.LocalStorage;
using LoginClassLibrary.Account;
using LoginClassLibrary.Login;
using SignalRBlazorApp.Services;
using System.Net;

namespace SignalRBlazorApp.Services
{
    /// <summary>
    /// 当收到未验证的statcode时，发送refreshToken验证并刷新新的Token和refreshToken.
    /// </summary>
    /// <param name="accountService"></param>
    /// <param name="localStorageService"></param>
    public class CustomHttpHandler(IUser accountService, ILocalStorageService localStorageService):DelegatingHandler
    {
        private const string LocalStorageKey = "auth";
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("Login");
            bool refreshTokenUrl = request.RequestUri!.AbsoluteUri.Contains("RefreshToken");
            
            if(loginUrl || refreshTokenUrl)
            {
                return await base.SendAsync(request, cancellationToken);
            }
            var resutl = await base.SendAsync(request, cancellationToken);
            if(resutl.StatusCode == HttpStatusCode.Unauthorized)
            {
                var stringTokn = await localStorageService.GetItemAsStringAsync(LocalStorageKey);
                if (stringTokn == null)
                {
                    return resutl;
                }
                string token = string.Empty;
                try
                {
                    token = request.Headers.Authorization!.Parameter!;
                }
                catch { }

                var deserializedToken = Serializations.DeserializeJsonString<UserSession>(stringTokn);
                if(deserializedToken is null)
                {
                    return resutl;
                }
                
                if(string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                var newJwtToken = await GetReshToken(deserializedToken.RefreshToken!);
                if(string.IsNullOrEmpty(newJwtToken))
                {
                    return resutl;
                }

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",newJwtToken);
                return await base.SendAsync(request,cancellationToken);
            }
            return resutl; 
        }

        private async Task<string> GetReshToken(string refreshToken)
        {
            var result = await accountService.RefreshTokenAsync(new RefreshToken() { Token = refreshToken});
            string serializedToken = Serializations.SerializeOjb(new UserSession()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            });
            await localStorageService.SetItemAsStringAsync(LocalStorageKey, serializedToken);
            return  result.Token;
        }
    }
}

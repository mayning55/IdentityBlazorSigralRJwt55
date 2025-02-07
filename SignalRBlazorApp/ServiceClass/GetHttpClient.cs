using Blazored.LocalStorage;
using LoginClassLibrary.Account;
using System.Net.Http;


namespace SignalRBlazorApp.Services
{
    /// <summary>
    /// 访问权限内容带上Token
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <param name="localStorageService"></param>
    public class GetHttpClient(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
    {
        private const string LocalStorageKey = "auth";
        public const string HeaderKey = "Authorization";
        public async Task<HttpClient> GetPrivateHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            var stringToken = await localStorageService.GetItemAsStringAsync(LocalStorageKey);
            if (string.IsNullOrEmpty(stringToken))
            {
                return client;
            }
            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if (deserializeToken == null)
            {
                return client;
            }
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializeToken.Token);
            return client;
        }

        public HttpClient GetPublicHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }
    }
}

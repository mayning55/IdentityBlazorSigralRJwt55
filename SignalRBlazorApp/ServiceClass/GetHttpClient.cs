using LoginClassLibrary.Account;


namespace SignalRBlazorApp.Services
{
    /// <summary>
    /// 通过IHttpClientFactory重构HttpClient实例,访问需要验证的链接时带上从本地存储读取到的Token。
    /// 否岀不带Token
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <param name="localStorageService"></param>
    public class GetHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
    {
        public const string HeaderKey = "auth";
        public async Task<HttpClient> GetPrivateHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            var stringToken = await localStorageService.GetToken();
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

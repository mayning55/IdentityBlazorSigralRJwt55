using Blazored.LocalStorage;

namespace SignalRBlazorApp.Services
{
    /// <summary>
    /// 通过ILocalStorageService构造新实例，实现读取，存储和删除的操作
    /// </summary>
    /// <param name="localStorageService"></param>
    public class LocalStorageService(ILocalStorageService localStorageService)
    {
        public const string StorageKey = "auth";
        public async Task<string> GetToken()
        {
            return await localStorageService.GetItemAsStringAsync(StorageKey);
        }
        public async Task SetToken(string item)
        {
            await localStorageService.SetItemAsStringAsync(StorageKey, item);
        }
        public async Task RemoveToken()
        {
            await localStorageService.RemoveItemAsync(StorageKey);
        }
    }
}

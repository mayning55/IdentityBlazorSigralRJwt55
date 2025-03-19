using System.Net.Http.Json;
using DateClassLibrary;
using SignalRBlazorApp.Services;

namespace SignalRBlazorApp.ServiceClass;
/// <summary>
/// Client各操作的具体实现方法。也可以把过程写在Blazor组件，如PersonList
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="getHttpClient"></param>

public class ClientImplementation<T>(GetHttpClient getHttpClient) : IClientDataInterface<T>
{
    public async Task<Responses> AddItem(T item, string requestUrl)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClient();
        var responses = await httpClient.PostAsJsonAsync($"{requestUrl}/AddItem", item);
        var result = await responses.Content.ReadFromJsonAsync<Responses>();
        return result;
    }

    public async Task<Responses> DeleteItem(string requestUrl,long id)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClient();
        var responses = await httpClient.DeleteAsync($"{requestUrl}/DeleteItem?id={id}");
        var result = await responses.Content.ReadFromJsonAsync<Responses>();
        return result;
    }

    public async Task<Responses> EditItem(T item, string requestUrl)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClient();
        var responses = await httpClient.PutAsJsonAsync($"{requestUrl}/EditItem",item);
        var result = await responses.Content.ReadFromJsonAsync<Responses>();
        return result;
    }

    public async Task<List<T>> GetItems(string requestUrl)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClient();
        return await httpClient.GetFromJsonAsync<List<T>>($"{requestUrl}/GetItems");
    }

    public async Task<T> GetItemById(string requestUrl,long id)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClient();
        return await httpClient.GetFromJsonAsync<T>($"{requestUrl}/GetItemById?id={id}");
    }
}

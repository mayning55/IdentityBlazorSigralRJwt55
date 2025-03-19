namespace DateClassLibrary;
/// <summary>
/// 接口用于Client
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IClientDataInterface<T>
{
    Task<List<T>> GetItems(string requestUrl);
    Task<T> GetItemById(string requestUrl,long id);
    Task<Responses> AddItem(T item, string requestUrl);
    Task<Responses> EditItem(T item, string requestUrl);
    Task<Responses> DeleteItem(string requestUrl,long id);

}

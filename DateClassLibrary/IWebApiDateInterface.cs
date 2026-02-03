namespace DateClassLibrary
{
    /// <summary>
    /// 接口实现依赖关系抽象化，用于控制器，这里使用Department为例，Person则使用EF的基本方式实现。
    /// https://learn.microsoft.com/zh-cn/dotnet/core/extensions/dependency-injection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWebApiDataInterface<T>
    {
        Task<List<T>> GetItems();
        Task<string> GetById(long id);
        //Task<T> GetByName(string name);//32循环！！！
        Task<T> GetItemById(long id);
        Task<IEnumerable<T>> SearchByName(string name);
        Task<Responses> AddItem(T item);
        Task<Responses> EditItem(T item);
        Task<Responses> DeleteItem(long id);

    }
}

namespace DateClassLibrary
{
    /// <summary>
    /// 接口实现依赖关系抽象化，这里使用Department为例，Person则还是使用EF的基本方式实现。
    /// https://learn.microsoft.com/zh-cn/dotnet/core/extensions/dependency-injection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWebApiDateInterface<T>
    {
        Task<List<T>> GetAll();
        Task<string> GetById(long id);
        // Task<T> GetById(long id);
        Task<IEnumerable<T>> SearchByName(string name);
        Task<Responses> AddItem(T item);
        Task<Responses> EditItem(T item);
        Task<Responses> DeleteItem(long id);

    }
}

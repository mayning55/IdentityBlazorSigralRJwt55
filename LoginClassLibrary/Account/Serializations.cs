using System.Text.Json;

namespace LoginClassLibrary.Account
{
    /// <summary>
    /// Json序列化，参阅：https://learn.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json/how-to
    /// </summary>
    public class Serializations
    {
        public static string SerializeOjb<T>(T modelObject) => JsonSerializer.Serialize(modelObject);
        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString);
        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString);

    }
}

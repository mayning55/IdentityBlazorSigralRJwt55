using System.Text.Json;

namespace LoginClassLibrary.Account
{
    /// <summary>
    /// 对象序列化和反序列化
    /// </summary>
    public class Serializations
    {
        public static string SerializeOjb<T>(T modelObject) => JsonSerializer.Serialize(modelObject);
        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString);
        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString);

    }
}

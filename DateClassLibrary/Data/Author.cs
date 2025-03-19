using System.Text.Json.Serialization;

namespace DateClassLibrary.Data;
/// <summary>
/// [JsonIgnore]避免循环32次，
/// 
/// </summary>
public class Author
{
    public long Id { get; set; }
    public string AuthorName { get; set; }
    [JsonIgnore]
    public ICollection<Book> Books { get; } = new List<Book>();
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace DateClassLibrary.Data;
/// <summary>
/// DateOnly:参阅： https://learn.microsoft.com/zh-cn/dotnet/standard/datetime/how-to-use-dateonly-timeonly
/// </summary>
public class Book
{
    public long Id { get; set; }
    public string Title { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode = true)]
    public DateTime? PublicationDate { get; set; }= DateTime.UtcNow.AddHours(8);
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    //可选项
    public long? AuthorId { get; set; }
    public Author? Author { get; set; }

    /*必选项
    public long AuthorId{get; set; }
    public Author Author{ get; set; } = null!;
    */
    public List<Comment> Comments { get; set; } = new List<Comment>();
}
public class Comment
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public Book? Book { get; set; }
    public string Message { get; set; }

}

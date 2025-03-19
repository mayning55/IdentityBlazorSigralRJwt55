using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DateClassLibrary.Data;

namespace ClassLibrary.Settings
{
    /// <summary>
    /// 实体类型配置
    /// </summary>
    public class ClassConfig
    {
        public class PersonConfig : IEntityTypeConfiguration<Person>
        {
            public void Configure(EntityTypeBuilder<Person> builder)
            {
                builder.ToTable("Persons");
                builder.Property(p => p.Number).IsRequired();
                builder.Property(p => p.FirstName).IsRequired();
            }
        }
        public class DepartmentConfig : IEntityTypeConfiguration<Department>
        {
            public void Configure(EntityTypeBuilder<Department> builder)
            {
                builder.ToTable("Departments");
                builder.Property(d => d.Name).IsRequired();
            }
        }
        public class AuthorConfig : IEntityTypeConfiguration<Author>
        {
            public void Configure(EntityTypeBuilder<Author> builder)
            {
                builder.ToTable("Authors");
                builder.Property(a => a.AuthorName).IsRequired();
                builder.HasMany<Book>(b => b.Books)
                        .WithOne(b => b.Author)
                        .OnDelete(DeleteBehavior.Restrict);
            }
        }
        public class BookConfig : IEntityTypeConfiguration<Book>
        {
            /// <summary>
            /// 一对多关系，无需级联删除。
            /// https://learn.microsoft.com/zh-cn/ef/core/modeling/relationships/one-to-many
            /// 级联删除，参阅：
            /// https://learn.microsoft.com/zh-cn/ef/core/saving/cascade-delete
            /// </summary>
            /// <param name="builder"></param>
            public void Configure(EntityTypeBuilder<Book> builder)
            {
                builder.ToTable("Books");
                builder.Property(a => a.Title).IsRequired();
            }
        }
        public class CommentConfig : IEntityTypeConfiguration<Comment>
        {
            public void Configure(EntityTypeBuilder<Comment> builder)
            {
                builder.ToTable("Comments");
                builder.Property(c => c.Message).IsRequired()
                        .IsUnicode()
                        .HasMaxLength(255);
                builder.HasOne<Book>(c => c.Book)
                        .WithMany(a => a.Comments)
                        .HasForeignKey(c => c.BookId)
                        .IsRequired();
                //无不特殊需求，可不必特殊外键。.HasForeignKey(c=>c.BookId)
            }
        }
    }
}

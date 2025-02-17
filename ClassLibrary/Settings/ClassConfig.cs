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
                builder.Property(p=>p.FirstName).IsRequired();
            }
        }
        public class DepartmentConfig : IEntityTypeConfiguration<Department>
        {
            public void Configure(EntityTypeBuilder<Department> builder)
            {
                builder.ToTable("Departments");
                builder.Property(d=>d.Name).IsRequired();
            }
        }
    }
}

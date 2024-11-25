using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
        }
    }
}

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Settings
{
    /// <summary>
    /// EF迁移用，迁移完可删除。
    /// </summary>
    public class EFCoreDBContextFactory:IDesignTimeDbContextFactory<EFCoreDBContext>
    {
        public EFCoreDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFCoreDBContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=IBSJ_DB;User Id=sa;Password=123;TrustServerCertificate=True;");
            return new EFCoreDBContext(optionsBuilder.Options);
        }

        //https://learn.microsoft.com/zh-cn/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
    }
}

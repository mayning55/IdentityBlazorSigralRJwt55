using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Settings
{
    /// <summary>
    /// 配置数据库的连接
    /// EF迁移用，迁移完可删除。
    /// </summary>
    public class EFCoreDBContextFactory:IDesignTimeDbContextFactory<EFCoreDBContext>
    {
        public EFCoreDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFCoreDBContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=IBSJ_DB;User Id=sa;Password=123123;TrustServerCertificate=True;");
            return new EFCoreDBContext(optionsBuilder.Options);
        }

        //https://learn.microsoft.com/zh-cn/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
    }
}

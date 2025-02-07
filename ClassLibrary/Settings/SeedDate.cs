using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary.Settings
{
    public static class SeedDate
    {
        /// <summary>
        /// 参阅：https://learn.microsoft.com/zh-cn/ef/core/modeling/data-seeding
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new EFCoreDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<EFCoreDBContext>>()))
            {
                if (dbContext == null)
                {
                    throw new ArgumentNullException("Null dbContext");
                }
                if (dbContext.Persons.Any())
                {
                    return;
                }
                dbContext.Persons.AddRange(
                    new Person
                    {
                        Number = "1001",
                        FirstName = "Jason"
                    },
                    new Person
                    {
                        Number = "1002",
                        FirstName = "Gillian"
                    },
                    new Person
                    {
                        Number = "1003",
                        FirstName = "Olin"
                    },
                    new Person
                    {
                        Number = "1004",
                        FirstName = "Aimee"
                    },
                    new Person
                    {
                        Number = "1005",
                        FirstName = "Julia"
                    },
                    new Person
                    {
                        Number = "1006",
                        FirstName = "Bertina"
                    },
                    new Person
                    {
                        Number = "1007",
                        FirstName = "Warlike"
                    },
                    new Person
                    {
                        Number = "1008",
                        FirstName = "Leanne"
                    },
                    new Person
                    {
                        Number = "1009",
                        FirstName = "Gregory"
                    }
                    );
                dbContext.SaveChanges();
            }
        }
    }
}

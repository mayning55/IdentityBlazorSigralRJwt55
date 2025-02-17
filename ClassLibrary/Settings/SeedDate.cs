using DateClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary.Settings
{
    /// <summary>
    /// 种子数据；初始化Person和Department信息。
    /// </summary>
    public static class SeedDate
    {
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
        public static void InitializeDep(IServiceProvider serviceProvider)
        {
            using (var dbContext = new EFCoreDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<EFCoreDBContext>>()))
            {
                if (dbContext == null)
                {
                    throw new ArgumentNullException("Null dbContext");
                }
                if (dbContext.Departments.Any())
                {
                    return;
                }
                dbContext.Departments.AddRange(
                    new Department
                    {
                        Name = "D1"
                    },
                    new Department
                    {
                        Name = "D2"
                    },
                    new Department
                    {
                        Name = "D3"
                    });
                dbContext.SaveChanges();
            }
        }
        public static void InitializeDepPerson(IServiceProvider serviceProvider)
        {
            using (var dbContext = new EFCoreDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<EFCoreDBContext>>()))
            {
                if (dbContext == null)
                {
                    throw new ArgumentNullException("Null dbContext");
                }
                if (dbContext.DepPersons.Any())
                {
                    return;
                }
                dbContext.DepPersons.AddRange(
                    new DepPerson
                    {
                        DepartmentId = 1,
                        PersonId = 1
                    },
                    new DepPerson
                    {
                        DepartmentId = 2,
                        PersonId = 1
                    },
                    new DepPerson
                    {
                        DepartmentId = 3,
                        PersonId = 1
                    },
                    new DepPerson
                    {
                        DepartmentId = 2,
                        PersonId = 2
                    });
                dbContext.SaveChanges();
            }
        }
    }
}

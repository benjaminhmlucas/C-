namespace SalesApp.Migrations
{
    using SalesApp.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SalesApp.Data.SalesContext>
    {
        public Configuration()
        {
            //set to false to have to type into Nuget console to update db.
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SalesApp.Data.SalesContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //  Use this space to create a list of things that won't change often(ie Countries/Regions)
            context.Regions.AddOrUpdate(
                p => p.Id,
                new SalesRegion {
                    Active = true,
                    Code = "NTH",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    Id = 1,
                    Name = "North Region",
                    SalesTarget = 10000.00M,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                },
                new SalesRegion {
                    Active = true,
                    Code = "STH",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    Id = 2,
                    Name = "South Region",
                    SalesTarget = 10000.00M,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                },
                new SalesRegion {
                    Active = true,
                    Code = "WST",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    Id = 3,
                    Name = "West Region",
                    SalesTarget = 10000.00M,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                },
                new SalesRegion {
                    Active = true,
                    Code = "EST",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    Id = 4,
                    Name = "East Region",
                    SalesTarget = 10000.00M,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                }
            );

            context.People.AddOrUpdate(
                p => p.Id,
                new SalesPerson {
                    Active = true,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    FirstName= "Bob",
                    LastName = "Smith",
                    Id = 1,
                    RegionId = 1,
                    SalesTarget = 2000.00M,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                },
                new SalesPerson {
                    Active = true,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    FirstName= "Janet",
                    LastName = "Smith",
                    Id = 2,
                    RegionId = 2,
                    SalesTarget = 2000.00M,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                }, new SalesPerson {
                    Active = true,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    FirstName = "Billy",
                    LastName = "Jones",
                    Id = 3,
                    RegionId = 4,
                    SalesTarget = 2000.00M,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                },
                new SalesPerson {
                    Active = true,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    FirstName = "Gertrude",
                    LastName = "Gonzalez",
                    Id = 4,
                    RegionId = 3,
                    SalesTarget = 2000.00M,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now
                }
            );
            context.Sales.AddOrUpdate(
                p => p.Id,
                new Sale {
                    Amount = 321.45M,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    Date = new DateTime(2020, 2, 3),
                    Id = 1,
                    PersonId = 1,
                    RegionId = 1,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now,
                    Status = 0
                },
                new Sale {
                    Amount = 421.45M,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    Date = new DateTime(2020, 6, 5),
                    Id = 2,
                    PersonId = 2,
                    RegionId = 2,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now,
                    Status = 0
                },
                new Sale {
                    Amount = 521.45M,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    Date = new DateTime(2020, 2, 7),
                    Id = 3,
                    PersonId = 4,
                    RegionId = 3,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now,
                    Status = 0
                },
                new Sale {
                    Amount = 621.45M,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    Date = new DateTime(2020, 3, 3),
                    Id = 4,
                    PersonId = 3,
                    RegionId = 4,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now,
                    Status = 0
                }
            );
        }
    }
}

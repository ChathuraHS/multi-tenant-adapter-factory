using CheckoutWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CheckoutWebAPI
{
    public class CheckoutDbContext : DbContext
    {
        public CheckoutDbContext(DbContextOptions<CheckoutDbContext> dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                //as - If the cast succeeds → databaseCreator gets the object else databaseCreator becomes null.

                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables(); // Checks whether tables exist and creates tables based on model classes.
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}

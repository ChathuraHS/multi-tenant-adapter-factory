using Microsoft.EntityFrameworkCore;

namespace CartWebAPI
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> dbContextOptions) : base(dbContextOptions)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected CartDbContext()
        {
        }
    }
}

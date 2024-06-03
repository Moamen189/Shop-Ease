using Microsoft.EntityFrameworkCore;
using shoppingCart.Presentation.Models;

namespace shoppingCart.Presentation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
    }
}

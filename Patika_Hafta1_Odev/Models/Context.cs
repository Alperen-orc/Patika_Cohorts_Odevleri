using Microsoft.EntityFrameworkCore;

namespace Patika_Hafta1_Odev.Models
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context>options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace ProductWebAPI.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
        public DbSet<Shipping> Shippings { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

    }
}

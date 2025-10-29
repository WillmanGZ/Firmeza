using AdminManager.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminManager.Web.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Client> Clientes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sell> Sells { get; set; }
        public DbSet<SellProduct> SellProducts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

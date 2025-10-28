using AdminManager.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminManager.Web.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

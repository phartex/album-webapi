using albumwebapi.Models;
using Microsoft.EntityFrameworkCore;

namespace albumwebapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {      
        }

        public DbSet<Review> Reviews { get; set; }
    }
}

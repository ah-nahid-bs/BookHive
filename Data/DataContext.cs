using BookHive.Models;
using Microsoft.EntityFrameworkCore;
namespace BookHive.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}

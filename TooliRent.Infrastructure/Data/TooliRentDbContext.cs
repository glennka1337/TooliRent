using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Data
{
    public class TooliRentDbContext : DbContext
    {
        public TooliRentDbContext(DbContextOptions<TooliRentDbContext> options) : base(options) { }

        public DbSet<Tool> Tools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

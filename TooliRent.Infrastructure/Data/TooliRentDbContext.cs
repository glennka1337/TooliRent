using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;
using TooliRent.Core.Models.Enums;

namespace TooliRent.Infrastructure.Data
{
    public class TooliRentDbContext : DbContext
    {
        public TooliRentDbContext(DbContextOptions<TooliRentDbContext> options) : base(options) { }

        public DbSet<Tool> Tools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Standard" },
                new Category { Id = 2, Name = "Pro" },
                new Category { Id = 3, Name = "Garden" },
                new Category { Id = 4, Name = "Safety" }
            );

            // Users (BCrypt hashes for: Admin#123 / Member#123)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "$2a$10$LhTj3eQpC1Jp8wQvJfUeP.HO541xgs9LN7AyAcwzF2ioOs1Vh6xoG",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Username = "member",
                    Password = "$2a$10$7stg7wY0IY5Q39I2nq3eIee5rT1Er9YXrVjuVjYIYAgq9rK2Vy6s.",
                    Role = "Member"
                }
            );

            // Tools
            modelBuilder.Entity<Tool>().HasData(
                new Tool
                {
                    Id = 1,
                    Name = "Slagborr",
                    Description = "Kraftfull slagborr 18V",
                    CategoryId = 2,
                    IsActive = true
                },
                new Tool
                {
                    Id = 2,
                    Name = "Skruvdragare",
                    Description = "Kompakt skruvdragare",
                    CategoryId = 1,
                    IsActive = true
                },
                new Tool
                {
                    Id = 3,
                    Name = "Grästrimmer",
                    Description = "Batteridriven grästrimmer",
                    CategoryId = 3,
                    IsActive = true
                },
                new Tool
                {
                    Id = 4,
                    Name = "Skyddshjälm",
                    Description = "EN397 klassad hjälm",
                    CategoryId = 4,
                    IsActive = true
                }
            );

            // Bookings (static dates in UTC)
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    UserId = 2,          // member
                    ToolId = 1,          // Slagborr
                    StartDate = new DateTime(2025, 01, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 01, 13, 0, 0, 0, DateTimeKind.Utc),
                    Status = BookingStatus.Approved,
                    CreatedUtc = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                },
                new Booking
                {
                    Id = 2,
                    UserId = 1,          // admin
                    ToolId = 4,          // Skyddshjälm
                    StartDate = new DateTime(2025, 01, 12, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 01, 14, 0, 0, 0, DateTimeKind.Utc),
                    Status = BookingStatus.Requested,
                    CreatedUtc = new DateTime(2025, 01, 02, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}

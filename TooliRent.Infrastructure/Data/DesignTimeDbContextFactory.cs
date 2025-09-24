using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TooliRent.Infrastructure.Data
{
    /*public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TooliRentDbContext>
    {
        public TooliRentDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "TooliRent");
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=localhost;Database=TooliRentDB;Trusted_Connection=True;";

            var optionsBuilder = new DbContextOptionsBuilder<TooliRentDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TooliRentDbContext(optionsBuilder.Options);
        }
    }*/
}

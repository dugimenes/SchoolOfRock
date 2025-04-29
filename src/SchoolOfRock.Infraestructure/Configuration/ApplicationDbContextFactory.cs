using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SchoolOfRock.Infraestructure.Configuration
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            if (environment == "Development")
            {
                optionsBuilder.UseSqlite(configuration.GetConnectionString("SqliteConnection"));
            }
            else
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
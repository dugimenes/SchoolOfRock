using Microsoft.EntityFrameworkCore;
using SchoolOfRock.Shared.Configuration;

namespace Conteudo.Infra
{
    public class ApplicationDbContextFactory : DesignTimeDbContextFactoryBase<ConteudoDbContext>
    {
        //public AlunoDbContext CreateDbContext(string[] args)
        //{
        //    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        //        .Build();

        //    var optionsBuilder = new DbContextOptionsBuilder<AlunoDbContext>();

        //    if (environment == "Development")
        //    {
        //        optionsBuilder.UseSqlite(configuration.GetConnectionString("SqliteConnection"));
        //    }
        //    else
        //    {
        //        optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
        //    }

        //    return new AlunoDbContext(optionsBuilder.Options);
        //}

        protected override ConteudoDbContext CreateNewInstance(DbContextOptions<ConteudoDbContext> options)
        {
            return new ConteudoDbContext(options);
        }
    }
}
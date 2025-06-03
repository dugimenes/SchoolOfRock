using Microsoft.EntityFrameworkCore;
using SchoolOfRock.Shared.Configuration;

namespace Pagamento.Infra
{
    public class ApplicationDbContextFactory : DesignTimeDbContextFactoryBase<PagamentoDbContext>
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

        protected override PagamentoDbContext CreateNewInstance(DbContextOptions<PagamentoDbContext> options)
        {
            return new PagamentoDbContext(options);
        }
    }
}
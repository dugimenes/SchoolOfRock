using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SchoolOfRock.Shared.Configuration
{
    /// <summary>
    /// Classe base genérica para DesignTimeDbContextFactory.
    /// Qualquer DbContext concreto pode herdar dela e simplesmente
    /// implementar o método CreateNewInstance, passando seu próprio tipo.
    /// </summary>
    /// <typeparam name="TContext">O DbContext que será instanciado</typeparam>
    public abstract class DesignTimeDbContextFactoryBase<TContext>
        : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Abaixo definimos a lógica comum que:
        /// 1) Obtém a variável de ambiente "ASPNETCORE_ENVIRONMENT"
        /// 2) Constrói o IConfiguration (appsettings.json + appsettings.{Environment}.json)
        /// 3) Monta o DbContextOptionsBuilder<TContext> escolhendo provider conforme a environment
        /// 4) Chama CreateNewInstance para criar o contexto específico
        /// </summary>
        public TContext CreateDbContext(string[] args)
        {
            // 1) Descobrir a environment (Development, Staging, Production, etc.)
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                              ?? "Development";

            // 2) Construir o configuration a partir dos arquivos appsettings
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            // 3) Criar o builder de opções para o TContext
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            // 4) Lógica genérica para escolher provider 
            //    (no exemplo, SQLite para Dev e SQL Server para outro ambiente)
            if (environment == "Development")
            {
                var sqliteConn = configuration.GetConnectionString("SqliteConnection");
                if (string.IsNullOrEmpty(sqliteConn))
                    throw new InvalidOperationException("ConnectionString 'SqliteConnection' não encontrada.");

                optionsBuilder.UseSqlite(sqliteConn);
            }
            else
            {
                var sqlServerConn = configuration.GetConnectionString("SqlServerConnection");
                if (string.IsNullOrEmpty(sqlServerConn))
                    throw new InvalidOperationException("ConnectionString 'SqlServerConnection' não encontrada.");

                optionsBuilder.UseSqlServer(sqlServerConn);
            }

            // 5) Delegar à subclasse a criação da instância concreta de TContext
            return CreateNewInstance(optionsBuilder.Options);
        }

        /// <summary>
        /// Método abstrato que cada fábrica concreta terá que implementar.
        /// Deve retornar uma nova instância de TContext usando as opções fornecidas.
        /// </summary>
        /// <param name="options">Opções já configuradas no DbContextOptionsBuilder</param>
        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
    }
}

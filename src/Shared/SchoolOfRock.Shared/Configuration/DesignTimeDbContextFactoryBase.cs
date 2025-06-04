using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SchoolOfRock.Shared.Configuration
{
    /// <summary>
    /// Classe base genérica para DesignTimeDbContextFactory.
    /// Centraliza a lógica de:
    /// 1) Detectar ASPNETCORE_ENVIRONMENT
    /// 2) Carregar appsettings.json do projeto de API
    /// 3) Configurar o DbContextOptionsBuilder (SQLite vs SQL Server)
    /// 4) Apontar as Migrations para o assembly Shared
    /// 5) Invocar CreateNewInstance(options) na subclasse concreta
    /// </summary>
    /// <typeparam name="TContext">O DbContext que será instanciado</typeparam>
    public abstract class DesignTimeDbContextFactoryBase<TContext>
        : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            // 1) Descobrir a environment (Development, Staging, Production, etc.)
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                              ?? "Development";

            // 2) Como geralmente o comando 'dotnet ef' roda no diretório do projeto passado em --project,
            //    precisamos “voltar” para a pasta do projeto de API que contém o appsettings.json.
            //
            //    Exemplo de estrutura:
            //    /MinhaSolucao
            //      /SchoolOfRock.API          <-- Contém appsettings.json
            //      /SchoolOfRock.Shared       <-- Aqui está esta classe base
            //      /Aluno.Infra               <-- Possui AlunoDbContext e a sua factory concreta
            //      /School.Infra              <-- Possui SchoolDbContext e a sua factory concreta
            //      /Sales.Infra               <-- Possui SalesDbContext e a sua factory concreta
            //
            //    Se o CreateDbContext for invocado dentro de 'SchoolOfRock.Shared', Directory.GetCurrentDirectory()
            //    retornará algo como ".../SchoolOfRock.Shared". Então, precisamos subir um nível e ir para "SchoolOfRock.API".

            var sharedFolder = Directory.GetCurrentDirectory();
            var migrationsAssembly = "SchoolOfRock.Data";

            // Ajuste este caminho caso seu projeto API tenha nome ou estrutura diferente.
            // Aqui assumimos que o projeto de API chama-se “SchoolOfRock.API” e vive no mesmo nível que Shared.
            var apiProjectPath = Path.Combine(sharedFolder, "..", "SchoolOfRock.API");

            // 3) Construir o IConfiguration direcionado para a pasta do API
            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            // 4) Criar o builder de opções para o TContext
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            // 5) Lógica genérica de escolha de provider e apontar MigrationsAssembly para o Shared
            if (environment == "Development")
            {
                var sqliteConn = configuration.GetConnectionString("SqliteConnection");
                if (string.IsNullOrWhiteSpace(sqliteConn))
                    throw new InvalidOperationException("ConnectionString 'SqliteConnection' não encontrada.");

                optionsBuilder.UseSqlite(sqliteConn, sqlOptions =>
                {
                    // Indica que as migrations estão no assembly do projeto Shared
                    sqlOptions.MigrationsAssembly(migrationsAssembly);
                });
            }
            else
            {
                var sqlServerConn = configuration.GetConnectionString("SqlServerConnection");
                if (string.IsNullOrWhiteSpace(sqlServerConn))
                    throw new InvalidOperationException("ConnectionString 'SqlServerConnection' não encontrada.");

                optionsBuilder.UseSqlServer(sqlServerConn, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(migrationsAssembly);
                });
            }

            // 6) Delegar à subclasse a criação da instância concreta de TContext
            return CreateNewInstance(optionsBuilder.Options);
        }

        /// <summary>
        /// As subclasses concretas precisam implementar este método para instanciar
        /// o respectivo TContext usando as opções fornecidas.
        /// Exemplo: return new MeuDbContext(options);
        /// </summary>
        /// <param name="options">Opções já configuradas (UseSqlite ou UseSqlServer)</param>
        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
    }
}

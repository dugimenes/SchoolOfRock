using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conteudo.Infra.Seeders
{
    public static class HostSeederExtensions
    {
        public static async Task SeedDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var db = services.GetRequiredService<ConteudoDbContext>();

            // Executa migrations
            await db.Database.MigrateAsync();

            // Executa seeder de aulas (CSV externo)
            await CursoSeeder.SeedAsync(db);
            await AulaSeeder.SeedAsync(db);
        }
    }

}
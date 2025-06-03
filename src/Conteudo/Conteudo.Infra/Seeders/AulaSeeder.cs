using Conteudo.Domain.AggregateModel;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Conteudo.Infra.Seeders
{
    public static class AulaSeeder
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (await context.Set<Aula>().AnyAsync()) return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "aulas.csv");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo aulas.csv não encontrado em: {filePath}");

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var registros = csv.GetRecords<AulaCsv>();

            var aulas = registros.Select(r => new Aula(
                r.Titulo,
                r.Conteudo,
                r.CursoId,
                string.IsNullOrWhiteSpace(r.MaterialUrl) ? null : r.MaterialUrl
            )
            {
                Id = r.Id
            }).ToList();

            context.Set<Aula>().AddRange(aulas);
            await context.SaveChangesAsync();
        }
    }

    public class AulaCsv
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = default!;
        public string Conteudo { get; set; } = default!;
        public Guid CursoId { get; set; }
        public string? MaterialUrl { get; set; }
    }
}
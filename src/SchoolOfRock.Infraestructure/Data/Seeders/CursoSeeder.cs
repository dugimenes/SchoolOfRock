using CsvHelper;
using Microsoft.EntityFrameworkCore;
using SchoolOfRock.Domain.Entity;
using SchoolOfRock.Domain.ValueObjects;
using System.Globalization;
using System.Text;

namespace SchoolOfRock.Infraestructure.Data.Seeders
{
    public static class CursoSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Set<Curso>().AnyAsync()) return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "cursos.csv");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo cursos.csv não encontrado em: {filePath}");

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var registros = csv.GetRecords<CursoCsv>().ToList();

            var cursos = registros.Select(c => new Curso(c.Id, c.Nome, new ConteudoProgramatico(c.ConteudoProgramatico))).ToList();

            await context.Set<Curso>().AddRangeAsync(cursos);
            await context.SaveChangesAsync();
        }

        private class CursoCsv
        {
            public Guid Id { get; set; }
            public string Nome { get; set; } = default!;
            public string ConteudoProgramatico { get; set; } = default!;
        }
    }
}
using Conteudo.Application.Command;
using Conteudo.Application.Handlers;
using Conteudo.Domain.AggregateModel;
using Conteudo.Infra;
using Conteudo.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SchoolOfRock.Tests.Conteudo.Integration
{
    public class ConteudoIntegrationTests
    {
        private readonly DbContextOptions<ConteudoDbContext> _dbOptions;

        public ConteudoIntegrationTests()
        {
            _dbOptions = new DbContextOptionsBuilder<ConteudoDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Conteudo")
                .Options;
        }

        [Fact]
        public async Task AdicionarAula_Deve_Incluir_Nova_Aula_No_Curso_Existente()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var curso = new Curso(cursoId, "Curso de Integração", new ConteudoProgramatico("Conteúdo do curso."));

            // Adiciona um curso ao banco em memória para o teste
            await using (var context = new ConteudoDbContext(_dbOptions))
            {
                context.Cursos.Add(curso);
                await context.SaveChangesAsync();
            }

            var command = new AdicionarAulaCommand
            {
                CursoId = cursoId,
                Titulo = "Nova Aula de Teste",
                Conteudo = "Conteúdo da nova aula.",
                MaterialUrl = "http://example.com/material"
            };

            // Act
            await using (var context = new ConteudoDbContext(_dbOptions))
            {
                var repository = new CursoRepository(context);
                var handler = new AdicionarAulaCommandHandler(repository);
                await handler.Handle(command, CancellationToken.None);
            }

            // Assert
            await using (var context = new ConteudoDbContext(_dbOptions))
            {
                var cursoDoDb = await context.Cursos.Include(c => c.Aulas).FirstOrDefaultAsync(c => c.Id == cursoId);
                Assert.NotNull(cursoDoDb);
                Assert.Single(cursoDoDb.Aulas);
                Assert.Equal("Nova Aula de Teste", cursoDoDb.Aulas.First().Titulo);
            }
        }
    }
}

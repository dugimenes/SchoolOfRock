using Aluno.Application.Command;
using Aluno.Application.Dtos;
using Aluno.Application.Handlers;
using Aluno.Domain.AggregateModel;
using Aluno.Infra;
using Aluno.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SchoolOfRock.Tests.Aluno.Integration
{
    public class AlunoIntegrationTests
    {
        private readonly DbContextOptions<AlunoDbContext> _dbOptions;

        public AlunoIntegrationTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AlunoDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Aluno")
                .Options;
        }

        [Fact]
        public async Task GerarCertificado_Deve_Criar_Certificado_Quando_Matricula_Estiver_Concluida()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aluno = new global::Aluno.Domain.AggregateModel.Aluno(alunoId, "Carlos", "carlos@email.com");
            var matricula = new Matricula(cursoId, alunoId);

            matricula.ConfirmarPagamento(); // Status: Ativa
            matricula.Concluir();           // Status: Concluida

            await using (var context = new AlunoDbContext(_dbOptions))
            {
                context.Alunos.Add(aluno);
                context.Matriculas.Add(matricula);
                await context.SaveChangesAsync();
            }

            var command = new GerarCertificadoCommand { AlunoId = alunoId, CursoId = cursoId };
            CertificadoDto result;

            // Act
            await using (var context = new AlunoDbContext(_dbOptions))
            {
                var alunoRepository = new AlunoRepository(context);
                var certificadoRepository = new CertificadoRepository(context);
                var matriculaRepository = new MatriculaRepository(context);

                var handler = new GerarCertificadoCommandHandler(alunoRepository, certificadoRepository, matriculaRepository);
                result = await handler.Handle(command, CancellationToken.None);
            }

            // Assert
            await using (var context = new AlunoDbContext(_dbOptions))
            {
                Assert.NotNull(result);
                Assert.NotEqual(Guid.Empty, result.Id);
                var certificadoNoDb = await context.Certificados.FindAsync(result.Id);
                Assert.NotNull(certificadoNoDb);
                Assert.Equal(alunoId, certificadoNoDb.AlunoId);
                Assert.Equal(cursoId, certificadoNoDb.CursoId);
            }
        }

        [Fact]
        public async Task GerarCertificado_Deve_Lancar_Excecao_Quando_Matricula_Nao_Estiver_Concluida()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aluno = new global::Aluno.Domain.AggregateModel.Aluno(alunoId, "Jonas", "jonas@email.com");

            // Matrícula com status PendentePagamento
            var matricula = new Matricula(cursoId, alunoId);

            await using (var context = new AlunoDbContext(_dbOptions))
            {
                context.Alunos.Add(aluno);
                context.Matriculas.Add(matricula);
                await context.SaveChangesAsync();
            }

            var command = new GerarCertificadoCommand { AlunoId = alunoId, CursoId = cursoId };

            // Act & Assert
            await using (var context = new AlunoDbContext(_dbOptions))
            {
                var alunoRepository = new AlunoRepository(context);
                var certificadoRepository = new CertificadoRepository(context);
                var matriculaRepository = new MatriculaRepository(context);

                var handler = new GerarCertificadoCommandHandler(alunoRepository, certificadoRepository, matriculaRepository);

                var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
                Assert.Equal("A matrícula do aluno neste curso não está concluída.", exception.Message);
            }
        }
    }
}
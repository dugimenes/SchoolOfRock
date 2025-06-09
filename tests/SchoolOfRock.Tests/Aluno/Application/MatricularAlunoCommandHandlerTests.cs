using Aluno.Application.Command;
using Aluno.Application.Dtos;
using Aluno.Application.Handlers;
using Aluno.Domain.AggregateModel;
using Aluno.Infra;
using Aluno.Infra.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Pagamento.Infra;
using Pagamento.Infra.Repository;
using System.Data.Common;
using Xunit;

namespace SchoolOfRock.Tests.Aluno.Application
{
    public class MatricularAlunoCommandHandlerTests : IDisposable
    {
        private readonly AlunoDbContext _alunoDbContext;
        private readonly PagamentoDbContext _pagamentoDbContext;
        private readonly DbConnection _connection;
        private readonly MatricularAlunoCommandHandler _handler;

        private class TestSchemaBuilderDbContext : DbContext
        {
            public DbSet<global::Aluno.Domain.AggregateModel.Aluno> Alunos { get; set; }
            public DbSet<Matricula> Matriculas { get; set; }
            public DbSet<Certificado> Certificados { get; set; }
            public DbSet<global::Pagamento.Domain.AggregateModel.Pagamento> Pagamentos { get; set; }

            public TestSchemaBuilderDbContext(DbConnection connection)
                : base(new DbContextOptionsBuilder().UseSqlite(connection).Options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoDbContext).Assembly);
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoDbContext).Assembly);
            }
        }

        public MatricularAlunoCommandHandlerTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            using (var schemaBuilderContext = new TestSchemaBuilderDbContext(_connection))
            {
                schemaBuilderContext.Database.EnsureCreated();
            }

            var alunoDbOptions = new DbContextOptionsBuilder<AlunoDbContext>().UseSqlite(_connection).Options;
            _alunoDbContext = new AlunoDbContext(alunoDbOptions);

            var pagamentoDbOptions = new DbContextOptionsBuilder<PagamentoDbContext>().UseSqlite(_connection).Options;
            _pagamentoDbContext = new PagamentoDbContext(pagamentoDbOptions);

            var alunoRepository = new AlunoRepository(_alunoDbContext);
            var matriculaRepository = new MatriculaRepository(_alunoDbContext);
            var pagamentoRepository = new PagamentoRepository(_pagamentoDbContext);

            _handler = new MatricularAlunoCommandHandler(
                alunoRepository, matriculaRepository, pagamentoRepository, _alunoDbContext, _pagamentoDbContext
            );
        }

        [Fact]
        public async Task Handle_Deve_Completar_Transacao_e_Persistir_Todos_Os_Dados()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aluno = new global::Aluno.Domain.AggregateModel.Aluno(alunoId, "Eduardo", "email@gmail.com");

            _alunoDbContext.Alunos.Add(aluno);
            await _alunoDbContext.SaveChangesAsync();

            var dadosCartaoDto = new DadosCartaoDto { Numero = "1234", NomeTitular = "Teste", Expiracao = "12/25", Cvv = "123" };
            var command = new MatricularAlunoCommand
            {
                AlunoId = alunoId,
                CursoId = cursoId,
                DadosCartao = dadosCartaoDto
            };

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotEqual(Guid.Empty, resultado.MatriculaId);
            Assert.NotEqual(Guid.Empty, resultado.PagamentoId);

            var matriculaSalva = await _alunoDbContext.Matriculas.FindAsync(resultado.MatriculaId);
            var pagamentoSalvo = await _pagamentoDbContext.Pagamentos.FindAsync(resultado.PagamentoId);

            Assert.NotNull(matriculaSalva);
            Assert.NotNull(pagamentoSalvo);
            Assert.Equal(resultado.MatriculaId, pagamentoSalvo.MatriculaId);
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
using Aluno.Application.Handlers;
using Aluno.Domain.AggregateModel;
using Aluno.Domain.Enumerations;
using Aluno.Infra;
using Aluno.Infra.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pagamento.Application.Command;
using Pagamento.Application.Handlers;
using Pagamento.Infra.Repository;
using SchoolOfRock.Contracts.Pagamento.Events;
using Xunit;

namespace SchoolOfRock.Tests.Pagamento.Integration
{
    public class PagamentoAlunoIntegrationTests
    {
        [Fact]
        public async Task ConfirmarPagamento_Deve_Ativar_Matricula_Do_Aluno()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AlunoDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_PagamentoAluno")
                .Options;

            var alunoDbContext = new AlunoDbContext(options);
            var matriculaRepository = new MatriculaRepository(alunoDbContext);

            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var matricula = new Matricula(cursoId, alunoId);
            alunoDbContext.Matriculas.Add(matricula);
            await alunoDbContext.SaveChangesAsync();

            var mediatorMock = new Mock<IMediator>();
            var pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            var pagamento = new global::Pagamento.Domain.AggregateModel.Pagamento(matricula.Id);
            pagamento.Confirmar(); // Gera o evento de domínio

            pagamentoRepositoryMock.Setup(r => r.ObterPorIdAsync(pagamento.Id)).ReturnsAsync(pagamento);

            var confirmarPagamentoHandler = new ConfirmarPagamentoCommandHandler(pagamentoRepositoryMock.Object, mediatorMock.Object);
            var pagamentoConfirmadoHandler = new PagamentoConfirmadoHandler(matriculaRepository);

            var command = new ConfirmarPagamentoCommand { PagamentoId = pagamento.Id };

            // Act
            await confirmarPagamentoHandler.Handle(command, CancellationToken.None);
            await pagamentoConfirmadoHandler.Handle(new PagamentoConfirmadoEvent(matricula.Id), CancellationToken.None);

            // Assert
            var matriculaAtualizada = await alunoDbContext.Matriculas.FindAsync(matricula.Id);
            Assert.Equal(StatusMatricula.Ativa, matriculaAtualizada.Status);
        }
    }
}

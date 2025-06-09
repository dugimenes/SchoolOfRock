using Aluno.Application.Command;
using Aluno.Application.Handlers;
using Aluno.Infra.Repository;
using Moq;
using Xunit;

namespace SchoolOfRock.Tests.Aluno.Application
{
    public class ConcluirAulaCommandHandlerTests
    {
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly ConcluirAulaCommandHandler _handler;

        public ConcluirAulaCommandHandlerTests()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _handler = new ConcluirAulaCommandHandler(_alunoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Concluir_Aula_Com_Sucesso_Quando_Aluno_Existe()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var aluno = new global::Aluno.Domain.AggregateModel.Aluno(alunoId, "Paulo", "paulo@gmail.com");

            _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId))
                .ReturnsAsync(aluno);

            var command = new ConcluirAulaCommand { AlunoId = alunoId, AulaId = aulaId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _alunoRepositoryMock.Verify(r => r.Atualizar(aluno), Times.Once);
            _alunoRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
using Aluno.Application.Command;
using Aluno.Application.Handlers;
using Aluno.Infra.Repository;
using Moq;
using SchoolOfRock.Shared.Repository;
using Xunit;

namespace SchoolOfRock.Tests.Aluno.Application
{
    public class AtualizarAlunoCommandHandlerTests
    {
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AtualizarAlunoCommandHandler _handler;

        public AtualizarAlunoCommandHandlerTests()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // Configura o mock do repositório para retornar o mock da UnitOfWork
            _alunoRepositoryMock.SetupGet(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);

            _handler = new AtualizarAlunoCommandHandler(_alunoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Atualizar_Aluno_e_Retornar_True_Quando_Aluno_Existe()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var alunoExistente = new global::Aluno.Domain.AggregateModel.Aluno(alunoId, "Nome Antigo", "email.antigo@example.com");

            var command = new AtualizarAlunoCommand
            {
                Id = alunoId,
                Nome = "Nome Novo",
                Email = "email.novo@example.com"
            };

            _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId))
                .ReturnsAsync(alunoExistente);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(resultado);

            _alunoRepositoryMock.Verify(r => r.Atualizar(It.IsAny<global::Aluno.Domain.AggregateModel.Aluno>()), Times.Once);

            Assert.Equal("Nome Novo", alunoExistente.Nome);
            Assert.Equal("email.novo@example.com", alunoExistente.Email);

            // 4. Verifica se a Unit of Work foi chamada para salvar as alterações
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_False_Quando_Aluno_Nao_Existe()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var command = new AtualizarAlunoCommand
            {
                Id = alunoId,
                Nome = "Nome Qualquer",
                Email = "email@qualquer.com"
            };

            _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId))
                .ReturnsAsync((global::Aluno.Domain.AggregateModel.Aluno)null);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(resultado);

            _alunoRepositoryMock.Verify(r => r.Atualizar(It.IsAny<global::Aluno.Domain.AggregateModel.Aluno>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
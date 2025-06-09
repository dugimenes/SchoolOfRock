using Aluno.Application.Command;
using Aluno.Application.Handlers;
using Aluno.Infra.Repository;
using Moq;
using Xunit;

namespace SchoolOfRock.Tests.Aluno.Application
{
    public class MatricularAlunoCommandHandlerTests
    {
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly MatricularAlunoCommandHandler _handler;

        public MatricularAlunoCommandHandlerTests()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _handler = new MatricularAlunoCommandHandler(_alunoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Id_Da_Nova_Matricula_Quando_Aluno_Existe()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aluno = new global::Aluno.Domain.AggregateModel.Aluno(alunoId, "Eduardo", "email@gmail.com");

            _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId))
                .ReturnsAsync(aluno);

            var command = new MatricularAlunoCommand { AlunoId = alunoId, CursoId = cursoId };

            // Act
            var matriculaId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, matriculaId);
            _alunoRepositoryMock.Verify(r => r.Atualizar(It.IsAny<global::Aluno.Domain.AggregateModel.Aluno>()), Times.Once);
            _alunoRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Lancar_Excecao_Quando_Aluno_Nao_Existe()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId))
                .ReturnsAsync((global::Aluno.Domain.AggregateModel.Aluno)null);

            var command = new MatricularAlunoCommand { AlunoId = alunoId, CursoId = cursoId };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
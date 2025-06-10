using Aluno.Application.Command;
using Aluno.Application.Handlers;
using Aluno.Domain.AggregateModel;
using Aluno.Infra.Repository;
using MediatR;
using Moq;
using SchoolOfRock.Contracts.Aluno.Events;
using Xunit;

namespace SchoolOfRock.Tests.Aluno.Application
{
    public class MatricularAlunoCommandHandlerTests
    {
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly Mock<IMatriculaRepository> _matriculaRepositoryMock;
        private readonly MatricularAlunoCommandHandler _handler;
        private readonly Mock<IMediator> _mediatorMock;

        public MatricularAlunoCommandHandlerTests()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _matriculaRepositoryMock = new Mock<IMatriculaRepository>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new MatricularAlunoCommandHandler(_alunoRepositoryMock.Object, _mediatorMock.Object, _matriculaRepositoryMock.Object);
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

            _matriculaRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Matricula>()), Times.Once);
            _matriculaRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once); // Verifique no mock correto
            _mediatorMock.Verify(m => m.Publish(
                    It.Is<INotification>(n => n is AlunoMatriculadoEvent),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _alunoRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
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

            _mediatorMock.Verify(m => m.Publish(
                    It.IsAny<INotification>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Lancar_Excecao_Quando_Aluno_Ja_Esta_Matriculado()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aluno = new global::Aluno.Domain.AggregateModel.Aluno(alunoId, "Eduardo", "email@gmail.com");

            var matriculaExistente = new Matricula(cursoId, alunoId);

            _alunoRepositoryMock.Setup(r => r.ObterPorIdAsync(alunoId))
                .ReturnsAsync(aluno);

            _matriculaRepositoryMock.Setup(r => r.ObterPorAlunoECursoIdAsync(alunoId, cursoId))
                .ReturnsAsync(matriculaExistente);

            var command = new MatricularAlunoCommand { AlunoId = alunoId, CursoId = cursoId };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Aluno já está matriculado neste curso.", exception.Message);
            _matriculaRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Matricula>()), Times.Never);

            _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
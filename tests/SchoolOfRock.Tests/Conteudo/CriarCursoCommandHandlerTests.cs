using Conteudo.Application.Command;
using Conteudo.Application.Handlers;
using Conteudo.Domain.AggregateModel;
using Conteudo.Infra.Repository;
using Moq;
using Xunit;

namespace SchoolOfRock.Tests.Conteudo
{
    public class CriarCursoCommandHandlerTests
    {
        private readonly Mock<ICursoRepository> _cursoRepositoryMock;
        private readonly CriarCursoCommandHandler _handler;

        public CriarCursoCommandHandlerTests()
        {
            _cursoRepositoryMock = new Mock<ICursoRepository>();
            _handler = new CriarCursoCommandHandler(_cursoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Criar_Curso_E_Retornar_Novo_Id()
        {
            // Arrange
            var command = new CriarCursoCommand
            {
                Nome = "Curso de Teste",
                ConteudoProgramatico = "Este é um curso de teste."
            };

            // Act
            var cursoId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, cursoId);
            _cursoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Curso>()), Times.Once);
            _cursoRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}

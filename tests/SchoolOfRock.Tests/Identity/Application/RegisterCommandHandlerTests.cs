using Identity.Application.Command;
using Identity.Application.Handlers;
using Identity.Domain.AggregateModel;
using Identity.Infra.Repository;
using MediatR;
using Moq;
using SchoolOfRock.Contracts.Identity.Events;
using Xunit;

namespace SchoolOfRock.Tests.Identity.Application
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new RegisterCommandHandler(_userRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Registrar_Usuario_Com_Sucesso()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Name = "Paulo Teste",
                Login = "teste@email.com",
                Password = "Password@123"
            };

            _userRepositoryMock.Setup(r => r.FindByEmailAsync(command.Login)).ReturnsAsync((ApplicationUser)null);

            _userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync((ApplicationUser user) =>
                {
                    user.Id = Guid.NewGuid().ToString();
                    return user;
                });

            // Act
            var userId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, userId);
            _userRepositoryMock.Verify(r => r.CreateAsync(It.Is<ApplicationUser>(u => u.Email == command.Login)), Times.Once);
            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

            _mediatorMock.Verify(m => m.Publish(
                    It.Is<INotification>(notification => notification is UsuarioCriadoEvent),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
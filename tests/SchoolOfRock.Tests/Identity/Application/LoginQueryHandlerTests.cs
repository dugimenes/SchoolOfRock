using Identity.Application.Handlers;
using Identity.Application.Queries;
using Identity.Domain.AggregateModel;
using Identity.Infra.Repository;
using Identity.Infra.Services;
using Moq;
using Xunit;

namespace SchoolOfRock.Tests.Identity.Application
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
        private readonly LoginQueryHandler _handler;

        public LoginQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _handler = new LoginQueryHandler(_userRepositoryMock.Object, _tokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Token_Para_Credenciais_Validas()
        {
            // Arrange
            var email = "usuario@gmail.com";
            var password = "Password@123";
            var user = new ApplicationUser { Email = email };
            var query = new LoginQuery { Email = email, Password = password };
            var token = "jwt-token-de-teste";

            _userRepositoryMock.Setup(r => r.FindByEmailAsync(email)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.CheckPasswordAsync(user, password)).ReturnsAsync(true);
            _tokenGeneratorMock.Setup(g => g.GerarToken(user)).ReturnsAsync(token);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Sucesso);
            Assert.Equal(token, result.Token);
        }
    }
}

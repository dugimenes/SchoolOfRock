using Identity.Application.Command;
using Identity.Application.Handlers;
using Identity.Domain.AggregateModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using Xunit;

public class RegisterCommandHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _mediatorMock = new Mock<IMediator>();
        _handler = new RegisterCommandHandler(_userManagerMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task Handle_Deve_Atribuir_Papel_Admin_Para_Primeiro_Usuario()
    {
        // Arrange
        var command = new RegisterCommand { Name = "Admin User", Login = "admin@example.com", Password = "Password123" };
        var users = new List<ApplicationUser>();
        var mockUsers = users.AsQueryable().BuildMock();

        _userManagerMock.Setup(x => x.Users).Returns(mockUsers);
        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Login)).ReturnsAsync((ApplicationUser)null);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), command.Password))
            .Callback<ApplicationUser, string>((user, pass) =>
            {
                user.Id = Guid.NewGuid().ToString();
                users.Add(user);
            })
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Admin"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var userId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, userId);

        _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Admin"), Times.Once);
        _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Normal"), Times.Never);
    }

    [Fact]
    public async Task Handle_Deve_Atribuir_Papel_Normal_Para_Usuarios_Subsequentes()
    {
        // Arrange
        var command = new RegisterCommand { Name = "Normal User", Login = "user@example.com", Password = "Password123" };
        var existingUsers = new List<ApplicationUser> { new ApplicationUser() };
        var mockUsers = existingUsers.AsQueryable().BuildMock();

        _userManagerMock.Setup(x => x.Users).Returns(mockUsers);
        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Login)).ReturnsAsync((ApplicationUser)null);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), command.Password))
            .Callback<ApplicationUser, string>((user, pass) =>
            {
                user.Id = Guid.NewGuid().ToString();
                existingUsers.Add(user);
            })
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Normal"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var userId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, userId);
        
        _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Normal"), Times.Once);
        _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Admin"), Times.Never);
    }
}
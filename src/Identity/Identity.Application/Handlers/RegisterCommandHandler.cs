using Identity.Application.Command;
using Identity.Domain.AggregateModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
    {
        private readonly UserManager<ApplicationUser> _userManager; // Use o UserManager
        private readonly IMediator _mediator;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Login);
            if (userExists != null)
            {
                throw new Exception("O e-mail já está em uso.");
            }

            var newUser = new ApplicationUser
            {
                UserName = request.Name,
                Email = request.Login,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                throw new Exception($"Falha ao registrar usuário: {errors}");
            }

            if (await _userManager.Users.CountAsync() == 1)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, "Normal");
            }

            newUser.Cadastrar();

            // A publicação de eventos continua a mesma
            foreach (var domainEvent in newUser.DomainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
            newUser.ClearDomainEvents();

            return Guid.Parse(newUser.Id);
        }
    }
}
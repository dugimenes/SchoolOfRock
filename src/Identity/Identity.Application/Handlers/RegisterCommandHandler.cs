using Identity.Application.Command;
using Identity.Domain.AggregateModel;
using Identity.Infra.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public RegisterCommandHandler(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.FindByEmailAsync(request.Login) != null)
            {
                throw new Exception("O e-mail já está em uso.");
            }

            var newUser = new ApplicationUser
            {
                UserName = request.Name,
                Email = request.Login,
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, request.Password);

            var result = await _userRepository.CreateAsync(newUser);
            
            newUser.Cadastrar();

            await _userRepository.SaveChangesAsync();

            foreach (var domainEvent in newUser.DomainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            newUser.ClearDomainEvents();

            return Guid.Parse(newUser.Id);
        }
    }
}

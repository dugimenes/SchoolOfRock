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
                throw new Exception("O e-mail já está em uso."); // Ou um resultado customizado
            }

            var newUser = new ApplicationUser
            {
                UserName = request.Name,
                Email = request.Login,
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, request.Password);

            var userSalvo = await _userRepository.CreateAsync(newUser);

            // Dispara o evento de integração para outros contextos (ex: AlunoContext) reagirem
            //var userRegisteredEvent = new UserRegisteredEvent(userSalvo.Id, userSalvo.UserName, userSalvo.Email);
            //await _mediator.Publish(userRegisteredEvent, cancellationToken);

            return Guid.Parse(userSalvo.Id);
        }
    }
}

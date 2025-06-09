using Identity.Application.Dtos;
using Identity.Application.Queries;
using Identity.Infra.Repository;
using Identity.Infra.Services;
using MediatR;

namespace Identity.Application.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginQueryHandler(IUserRepository userRepository, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResultDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);

            if (user == null || !await _userRepository.CheckPasswordAsync(user, request.Password))
            {
                return new LoginResultDto { Sucesso = false, Erro = "Usuário ou senha inválidos." };
            }

            var token = await _tokenGenerator.GerarToken(user);

            return new LoginResultDto { Sucesso = true, Token = token };
        }
    }
}
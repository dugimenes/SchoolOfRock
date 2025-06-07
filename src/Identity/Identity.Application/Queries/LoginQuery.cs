using Identity.Application.Dtos;
using MediatR;

namespace Identity.Application.Queries
{
    public class LoginQuery : IRequest<LoginResultDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
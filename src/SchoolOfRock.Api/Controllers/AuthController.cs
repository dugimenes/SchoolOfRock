using Identity.Application.Command;
using Identity.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolOfRock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.Sucesso)
            {
                return Unauthorized(new { message = result.Erro });
            }

            return Ok(new { token = result.Token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var userId = await _mediator.Send(command);
                // Retorna o ID do novo usuário criado
                return Ok(new { message = "Usuário registrado com sucesso.", userId });
            }
            catch (Exception ex)
            {
                // O handler lança uma exceção se o e-mail já existir
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
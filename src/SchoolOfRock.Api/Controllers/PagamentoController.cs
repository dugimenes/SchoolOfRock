using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagamento.Application.Command;

namespace SchoolOfRock.Api.Controllers
{
    [Route("api/[controller]")]
    public class PagamentoController : BaseController
    {
        private readonly IMediator _mediator;

        public PagamentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{matriculaId}/Confirmar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmarPagamento(Guid matriculaId)
        {
            var command = new ConfirmarPagamentoCommand { MatriculaId = matriculaId };
            var sucesso = await _mediator.Send(command);

            if (!sucesso)
            {
                return NotFound("Pagamento não encontrado.");
            }

            return Ok("Pagamento confirmado.");
        }

    }
}
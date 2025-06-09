using MediatR;
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

        [HttpPost("{pagamentoId}/confirmar")]
        public async Task<IActionResult> ConfirmarPagamento(Guid pagamentoId)
        {
            var command = new ConfirmarPagamentoCommand { PagamentoId = pagamentoId };
            var sucesso = await _mediator.Send(command);

            if (!sucesso)
            {
                return NotFound("Pagamento não encontrado.");
            }

            return Ok("Pagamento confirmado.");
        }

    }
}
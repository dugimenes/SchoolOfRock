using Aluno.Application.Command;
using Aluno.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SchoolOfRock.Api.Controllers
{
    [Route("api/[controller]")]
    public class CertificadoController : BaseController
    {
        private readonly IMediator _mediator;

        public CertificadoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GerarCertificado([FromBody] GerarCertificadoCommand command)
        {
            try
            {
                var certificadoDto = await _mediator.Send(command);
                return CreatedAtAction(nameof(ObterCertificadosPorAluno), new { alunoId = certificadoDto.AlunoId },
                    certificadoDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("aluno/{alunoId}")]
        public async Task<IActionResult> ObterCertificadosPorAluno(Guid alunoId)
        {
            var query = new ListarCertificadosPorAlunoQuery(alunoId);
            var certificados = await _mediator.Send(query);
            return Ok(certificados);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCertificado(Guid id, [FromBody] AtualizarCertificadoRequest request)
        {
            var command = new AtualizarCertificadoCommand
            {
                CertificadoId = id,
                NovaDataEmissao = request.NovaDataEmissao
            };

            try
            {
                var sucesso = await _mediator.Send(command);

                if (!sucesso)
                {
                    return NotFound(new { message = "Certificado não encontrado." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class AtualizarCertificadoRequest
    {
        public DateTime NovaDataEmissao { get; set; }
    }
}
using Conteudo.Application.Command;
using Conteudo.Application.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SchoolOfRock.Api.Controllers
{
    [Route("api/[controller]")]
    public class CursosController : BaseController
    {
        private readonly IMediator _mediator;

        public CursosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CriarCurso([FromBody] CriarCursoCommand command)
        {
            var cursoId = await _mediator.Send(command);
            return CreatedAtAction(nameof(ObterPorId), new { id = cursoId }, command);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var query = new ObterCursoPorIdQuery(id);
            var curso = await _mediator.Send(query);

            if (curso == null)
            {
                return NotFound();
            }

            return Ok(curso);
        }

        [HttpPost("{id}/aulas")]
        public async Task<IActionResult> AdicionarAula(Guid id, [FromBody] AdicionarAulaRequest request)
        {
            var command = new AdicionarAulaCommand
            {
                CursoId = id,
                Titulo = request.Titulo,
                Conteudo = request.Conteudo,
                MaterialUrl = request.MaterialUrl
            };

            try
            {
                var aulaId = await _mediator.Send(command);
                return Ok(new { message = "Aula adicionada com sucesso.", aulaId });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                return BadRequest(new { message = "Ocorreram erros de validação.", errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class AdicionarAulaRequest
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string MaterialUrl { get; set; }
    }
}
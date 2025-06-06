using Conteudo.Application.Command;
using Conteudo.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SchoolOfRock.Api.Controllers
{
    [Route("api/[controller]")]
    public class CursoController : BaseController
    {
        private readonly IMediator _mediator;
        public CursoController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        public async Task<IActionResult> CriarCurso([FromBody] CriarCursoCommand command)
        {
            var cursoId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = cursoId }, command);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new ObterMatriculaPorIdQuery(id);
            var matriculaDto = await _mediator.Send(query);

            if (matriculaDto == null)
            {
                return NotFound();
            }

            return Ok(matriculaDto);
        }
    }
}
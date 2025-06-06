using Aluno.Application.Command;
using Conteudo.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SchoolOfRock.Api.Controllers
{
    [Route("api/[controller]")]
    public class MatriculaController : BaseController
    {
        private readonly IMediator _mediator;
        public MatriculaController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        public async Task<IActionResult> RealizarMatricula([FromBody] RealizarMatriculaCommand command)
        {
            var matriculaId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = matriculaId }, command);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new ObterCursoPorIdQuery(id);
            var cursoDto = await _mediator.Send(query);

            if (cursoDto == null)
            {
                return NotFound();
            }

            return Ok(cursoDto);
        }
    }
}

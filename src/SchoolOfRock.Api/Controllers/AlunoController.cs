using Aluno.Application.Command;
using Aluno.Application.Dtos;
using Aluno.Application.Queries;
using Conteudo.Application.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolOfRock.Api.Controllers
{
    [Route("api/[controller]")]
    public class AlunoController : BaseController
    {
        private readonly IMediator _mediator;

        public AlunoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MatriculaRealizadaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var query = new ObterAlunoPorIdQuery(id);
            var aluno = await _mediator.Send(query);

            if (aluno == null)
            {
                return NotFound();
            }

            return Ok(aluno);
        }

        [HttpPost("{id}/Matricular")]
        [ProducesResponseType(typeof(AlunoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Matricular(Guid id, [FromBody] MatricularAlunoRequest request)
        {
            var command = new MatricularAlunoCommand
            {
                AlunoId = id,
                CursoId = request.CursoId,
                DadosCartao = request.DadosCartao
            };

            try
            {
                var matriculaId = await _mediator.Send(command);
                return Ok(new { message = "Matrícula realizada com sucesso.", matriculaId });
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

        [HttpDelete("RemoverMatricula/{matriculaId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletarMatricula(Guid matriculaId)
        {
            var command = new DeletarMatriculaCommand(matriculaId);
            var sucesso = await _mediator.Send(command);

            if (!sucesso)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("RemoverAluno/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var command = new DeletarAlunoCommand(id);
            var sucesso = await _mediator.Send(command);

            if (!sucesso)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}/ObterHistorico")]
        [ProducesResponseType(typeof(MatriculaRealizadaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterHistorico(Guid id)
        {
            var query = new ObterHistoricoAprendizadoQuery(id);
            var historico = await _mediator.Send(query);

            if (historico == null)
            {
                return NotFound("Aluno ou histórico não encontrado.");
            }

            return Ok(historico);
        }

        [HttpPut("Atualizar{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarAlunoRequest request)
        {
            var command = new AtualizarAlunoCommand
            {
                Id = id,
                Nome = request.Nome,
                Email = request.Email
            };

            try
            {
                var sucesso = await _mediator.Send(command);

                if (!sucesso)
                {
                    return NotFound(new { message = "Aluno não encontrado." });
                }

                return NoContent();
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

        [HttpPost("{alunoId}/Aula/{aulaId}/Concluir")]
        [ProducesResponseType(typeof(AlunoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConcluirAula(Guid alunoId, Guid aulaId)
        {
            var command = new ConcluirAulaCommand
            {
                AlunoId = alunoId,
                AulaId = aulaId
            };

            var sucesso = await _mediator.Send(command);

            if (!sucesso)
            {
                return BadRequest("Não foi possível concluir a aula. Verifique se o aluno existe.");
            }

            return Ok(new { message = "Aula concluída com sucesso." });
        }
    }

    public class MatricularAlunoRequest
    {
        public Guid CursoId { get; set; }

        public DadosCartaoDto DadosCartao { get; set; }
    }

    public class AtualizarAlunoRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
using Aluno.Application.Command;
using Aluno.Application.Dtos;
using Aluno.Application.Queries;
using Conteudo.Application.Queries;
using FluentValidation;
using MediatR;
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

        [HttpPost("{id}/matricular")]
        public async Task<IActionResult> Matricular(Guid id, [FromBody] MatricularAlunoRequest request)
        {
            var command = new MatricularAlunoCommand
            {
                AlunoId = id,
                CursoId = request.CursoId
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

        [HttpGet("{id}/historico")]
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

        [HttpPost("{alunoId}/aulas/{aulaId}/concluir")]
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
}
using Aluno.Application.Dtos;
using Aluno.Application.Queries;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class ObterHistoricoAprendizadoQueryHandler : IRequestHandler<ObterHistoricoAprendizadoQuery, HistoricoAprendizadoDto>
    {
        private readonly IAlunoRepository _alunoRepository;

        public ObterHistoricoAprendizadoQueryHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<HistoricoAprendizadoDto> Handle(ObterHistoricoAprendizadoQuery request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(request.AlunoId);
            if (aluno?.HistoricoAprendizado == null)
            {
                return null;
            }

            // Mapeia o Value Object para o DTO
            var historicoDto = new HistoricoAprendizadoDto
            {
                AlunoId = aluno.Id,
                AulasConcluidas = aluno.HistoricoAprendizado.AulasConcluidas.Select(ac => new AulaConcluidaDto
                {
                    AulaId = ac.AulaId,
                    DataConclusao = ac.DataConclusao
                }).ToList()
            };

            return historicoDto;
        }
    }
}

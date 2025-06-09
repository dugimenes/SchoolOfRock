using Aluno.Application.Dtos;
using MediatR;

namespace Aluno.Application.Queries
{
    public class ObterHistoricoAprendizadoQuery : IRequest<HistoricoAprendizadoDto>
    {
        public Guid AlunoId { get; }

        public ObterHistoricoAprendizadoQuery(Guid alunoId)
        {
            AlunoId = alunoId;
        }
    }
}

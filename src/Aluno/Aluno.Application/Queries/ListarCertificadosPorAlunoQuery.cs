using Aluno.Application.Dtos;
using MediatR;

namespace Aluno.Application.Queries
{
    public class ListarCertificadosPorAlunoQuery : IRequest<List<CertificadoDto>>
    {
        public Guid AlunoId { get; }

        public ListarCertificadosPorAlunoQuery(Guid alunoId)
        {
            AlunoId = alunoId;
        }
    }
}

using Aluno.Application.Dtos;
using MediatR;

namespace Aluno.Application.Command
{
    public class GerarCertificadoCommand : IRequest<CertificadoDto>
    {
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
    }
}

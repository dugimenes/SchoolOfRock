using MediatR;

namespace Aluno.Application.Command
{
    public class RealizarMatriculaCommand : IRequest<Guid>
    {
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
    }
}

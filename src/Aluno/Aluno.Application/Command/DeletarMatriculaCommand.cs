using MediatR;

namespace Aluno.Application.Command
{
    public class DeletarMatriculaCommand : IRequest<bool>
    {
        public Guid MatriculaId { get; set; }

        public DeletarMatriculaCommand(Guid matriculaId)
        {
            MatriculaId = matriculaId;
        }
    }
}

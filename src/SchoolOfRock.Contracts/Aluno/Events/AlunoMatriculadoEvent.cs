using MediatR;

namespace SchoolOfRock.Contracts.Aluno.Events
{
    public class AlunoMatriculadoEvent : INotification
    {
        public Guid MatriculaId { get; }
        public Guid AlunoId { get; }
        public Guid CursoId { get; }

        public AlunoMatriculadoEvent(Guid matriculaId, Guid alunoId, Guid cursoId)
        {
            MatriculaId = matriculaId;
            AlunoId = alunoId;
            CursoId = cursoId;
        }
    }
}

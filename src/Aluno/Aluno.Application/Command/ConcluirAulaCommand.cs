using MediatR;

namespace Aluno.Application.Command
{
    public class ConcluirAulaCommand : IRequest<bool>
    {
        public Guid AlunoId { get; set; }
        public Guid AulaId { get; set; }
    }
}

using MediatR;

namespace Aluno.Application.Command
{
    public class DeletarAlunoCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeletarAlunoCommand(Guid id)
        {
            Id = id;
        }
    }
}

using MediatR;

namespace Aluno.Application.Command
{
    public class AtualizarAlunoCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }
        public string Email { get; set; }
    }
}

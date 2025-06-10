using MediatR;

namespace Conteudo.Application.Command
{
    public class DeletarCursoCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeletarCursoCommand(Guid id)
        {
            Id = id;
        }
    }
}

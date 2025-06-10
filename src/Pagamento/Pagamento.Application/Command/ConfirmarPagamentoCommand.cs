using MediatR;

namespace Pagamento.Application.Command
{
    public class ConfirmarPagamentoCommand : IRequest<bool>
    {
        public Guid MatriculaId { get; set; }

    }
}
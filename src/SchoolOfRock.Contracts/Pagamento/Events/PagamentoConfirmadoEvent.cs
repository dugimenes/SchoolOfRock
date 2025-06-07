using MediatR;

namespace SchoolOfRock.Contracts.Pagamento.Events
{
    public class PagamentoConfirmadoEvent : INotification
    {
        public Guid MatriculaId { get; }

        public PagamentoConfirmadoEvent(Guid matriculaId) { MatriculaId = matriculaId; }

    }
}
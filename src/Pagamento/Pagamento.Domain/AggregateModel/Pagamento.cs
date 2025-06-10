using MediatR;
using SchoolOfRock.Contracts.Pagamento.Events;
using SchoolOfRock.Shared;

namespace Pagamento.Domain.AggregateModel
{
    public class Pagamento : Entity, IHasDomainEvents
    {
        private readonly List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();


        public Guid MatriculaId { get; private set; }
        public StatusPagamento StatusPagamento { get; private set; }

        protected Pagamento() { }

        public Pagamento(Guid matriculaId)
        {
            MatriculaId = matriculaId;
            StatusPagamento = StatusPagamento.Pendente;
        }

        public void Confirmar()
        {
            if (StatusPagamento == StatusPagamento.Confirmado)
            {
                return;
            }

            StatusPagamento = StatusPagamento.Confirmado;

            this.AddDomainEvent(new PagamentoConfirmadoEvent(this.MatriculaId));
        }

        public void Rejeitar()
        {
            StatusPagamento = StatusPagamento.Rejeitado;
        }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }

    public interface IHasDomainEvents
    {
        public IReadOnlyCollection<INotification> DomainEvents { get; }
        public void AddDomainEvent(INotification eventItem);
        public void ClearDomainEvents();
    }
}
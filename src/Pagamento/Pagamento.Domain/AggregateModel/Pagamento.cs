using SchoolOfRock.Shared;

namespace Pagamento.Domain.AggregateModel
{
    public class Pagamento : Entity
    {
        public Guid MatriculaId { get; private set; }
        public StatusPagamento StatusPagamento { get; private set; }

        protected Pagamento() { }

        public Pagamento(Guid matriculaId)
        {
            MatriculaId = matriculaId;
            StatusPagamento = StatusPagamento.Pendente;
        }

        public void Confirmar() => StatusPagamento = StatusPagamento.Confirmado;
        public void Rejeitar() => StatusPagamento = StatusPagamento.Rejeitado;
    }
}
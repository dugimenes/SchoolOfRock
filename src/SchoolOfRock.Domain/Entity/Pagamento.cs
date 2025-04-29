using SchoolOfRock.Domain.Models;
using SchoolOfRock.Domain.ValueObjects;

namespace SchoolOfRock.Domain.Entity
{
    public class Pagamento : Common.Entity
    {
        public Guid MatriculaId { get; private set; }
        public DadosCartao DadosCartao { get; private set; }
        public StatusPagamento StatusPagamento { get; private set; }

        protected Pagamento() { }

        public Pagamento(Guid matriculaId, DadosCartao dadosCartao)
        {
            MatriculaId = matriculaId;
            DadosCartao = dadosCartao;
            StatusPagamento = StatusPagamento.Pendente;
        }

        public void Confirmar() => StatusPagamento = StatusPagamento.Confirmado;
        public void Rejeitar() => StatusPagamento = StatusPagamento.Rejeitado;
    }
}
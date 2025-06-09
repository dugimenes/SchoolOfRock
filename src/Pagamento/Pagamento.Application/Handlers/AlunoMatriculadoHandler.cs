using MediatR;
using Pagamento.Infra.Repository;
using SchoolOfRock.Contracts.Aluno.Events;

namespace Pagamento.Application.Handlers
{
    public class AlunoMatriculadoHandler : INotificationHandler<AlunoMatriculadoEvent>
    {
        private readonly IPagamentoRepository _pagamentoRepository;

        public AlunoMatriculadoHandler(IPagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task Handle(AlunoMatriculadoEvent notification, CancellationToken cancellationToken)
        {
            var novoPagamento = new Domain.AggregateModel.Pagamento(notification.MatriculaId);

            await _pagamentoRepository.AdicionarAsync(novoPagamento);
            await _pagamentoRepository.SaveChangesAsync();

            // Getway de pagamento pode ser chamado aqui para processar o pagamento
        }
    }
}
using MediatR;
using Pagamento.Application.Command;
using Pagamento.Infra.Repository;

namespace Pagamento.Application.Handlers
{
    public class ConfirmarPagamentoCommandHandler : IRequestHandler<ConfirmarPagamentoCommand, bool>
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMediator _mediator;

        public ConfirmarPagamentoCommandHandler(IPagamentoRepository pagamentoRepository, IMediator mediator)
        {
            _pagamentoRepository = pagamentoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(ConfirmarPagamentoCommand request, CancellationToken cancellationToken)
        {
            // 1. Carregar o agregado do banco de dados
            var pagamento = await _pagamentoRepository.ObterPorIdAsync(request.PagamentoId);

            if (pagamento == null)
            {
                // Pagamento não encontrado, retorna falha
                return false;
            }

            // 2. Executar a regra de negócio na entidade de domínio
            // A entidade, ao executar, irá gerar e registrar o evento internamente.
            pagamento.Confirmar();

            // 3. Persistir a alteração no banco
            // O SaveChangesAsync() irá salvar o novo status do pagamento
            await _pagamentoRepository.SaveChangesAsync();


            // 4. *** DISPARAR O EVENTO REGISTRADO ***
            // Após a transação do banco ser confirmada com sucesso,
            // iteramos sobre os eventos registrados na entidade e os disparamos usando o MediatR.
            // Isso garante que os "ouvintes" só sejam notificados se a operação principal tiver sucesso.
            foreach (var domainEvent in pagamento.DomainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            // Limpa os eventos da entidade para não serem disparados novamente
            pagamento.ClearDomainEvents();

            return true;
        }
    }
}

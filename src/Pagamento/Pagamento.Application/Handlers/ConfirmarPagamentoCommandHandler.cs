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
            var pagamento = await _pagamentoRepository.ObterPagamentoMatriculaPendenteAsync(request.MatriculaId);

            if (pagamento == null)
            {
                throw new Exception("Não existe pagamento pendente para essa matricula.");
            }

            pagamento.Confirmar();

            await _pagamentoRepository.SaveChangesAsync();

            foreach (var domainEvent in pagamento.DomainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            pagamento.ClearDomainEvents();

            return true;
        }
    }
}
using Aluno.Infra.Repository;
using MediatR;
using SchoolOfRock.Contracts.Pagamento.Events;

namespace Aluno.Application.Handlers
{
    public class PagamentoConfirmadoHandler : INotificationHandler<PagamentoConfirmadoEvent>
    {
        private readonly IMatriculaRepository _matriculaRepository;

        public PagamentoConfirmadoHandler(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository;
        }

        public async Task Handle(PagamentoConfirmadoEvent notification, CancellationToken cancellationToken)
        {
            var matricula = await _matriculaRepository.ObterPorIdAsync(notification.MatriculaId);
            if (matricula != null)
            {
                matricula.ConfirmarPagamento();
                _matriculaRepository.Atualizar(matricula);
            }
        }
    }
}

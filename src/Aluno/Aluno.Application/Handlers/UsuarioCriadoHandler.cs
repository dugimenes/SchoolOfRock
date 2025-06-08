using Aluno.Infra.Repository;
using MediatR;
using SchoolOfRock.Contracts.Identity.Events;

namespace Aluno.Application.Handlers
{
    public class UsuarioCriadoHandler : INotificationHandler<UsuarioCriadoEvent>
    {
        private readonly IAlunoRepository _alunoRepository;

        public UsuarioCriadoHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task Handle(UsuarioCriadoEvent notification, CancellationToken cancellationToken)
        {
            if (await _alunoRepository.ObterPorIdAsync(notification.UsuarioId) != null)
            {
                return;
            }

            var novoAluno = new Domain.AggregateModel.Aluno(notification.UsuarioId, notification.Nome, notification.Email);

            await _alunoRepository.AdicionarAsync(novoAluno);

            await _alunoRepository.SaveChangesAsync();
        }
    }
}

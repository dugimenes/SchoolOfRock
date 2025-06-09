using Aluno.Application.Command;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class ConcluirAulaCommandHandler : IRequestHandler<ConcluirAulaCommand, bool>
    {
        private readonly IAlunoRepository _alunoRepository;

        public ConcluirAulaCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<bool> Handle(ConcluirAulaCommand request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(request.AlunoId);
            if (aluno == null)
            {
                throw new Exception("Verifique o Id informado para o aluno.");
            }

            // A lógica de adicionar a aula concluída é delegada para a entidade de domínio
            aluno.ConcluirAula(request.AulaId, DateTime.UtcNow);

            _alunoRepository.Atualizar(aluno);
            await _alunoRepository.SaveChangesAsync();

            return true;
        }
    }
}

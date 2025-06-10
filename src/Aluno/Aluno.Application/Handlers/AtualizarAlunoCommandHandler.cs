using Aluno.Application.Command;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class AtualizarAlunoCommandHandler : IRequestHandler<AtualizarAlunoCommand, bool>
    {
        private readonly IAlunoRepository _alunoRepository;

        public AtualizarAlunoCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<bool> Handle(AtualizarAlunoCommand request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(request.Id);

            if (aluno == null)
            {
                return false;
            }

            aluno.AtualizarDados(request.Nome, request.Email);

            _alunoRepository.Atualizar(aluno);

            var sucesso = await _alunoRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;

            return sucesso;
        }
    }
}

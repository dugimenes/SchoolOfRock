using Aluno.Application.Command;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class DeletarAlunoCommandHandler : IRequestHandler<DeletarAlunoCommand, bool>
    {
        private readonly IAlunoRepository _alunoRepository;

        public DeletarAlunoCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<bool> Handle(DeletarAlunoCommand request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(request.Id);

            if (aluno == null)
            {
                return false;
            }

            _alunoRepository.Remover(aluno);

            return await _alunoRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
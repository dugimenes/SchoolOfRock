using Aluno.Application.Command;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class DeletarMatriculaCommandHandler : IRequestHandler<DeletarMatriculaCommand, bool>
    {
        private readonly IMatriculaRepository _matriculaRepository;

        public DeletarMatriculaCommandHandler(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository;
        }

        public async Task<bool> Handle(DeletarMatriculaCommand request, CancellationToken cancellationToken)
        {
            var matricula = await _matriculaRepository.ObterPorIdAsync(request.MatriculaId);

            if (matricula == null)
            {
                return false;
            }

            _matriculaRepository.Remover(matricula);

            return await _matriculaRepository.SaveChangesAsync() > 0;
        }
    }
}

using Conteudo.Application.Command;
using Conteudo.Infra.Repository;
using MediatR;

namespace Conteudo.Application.Handlers
{
    public class DeletarCursoCommandHandler : IRequestHandler<DeletarCursoCommand, bool>
    {
        private readonly ICursoRepository _cursoRepository;

        public DeletarCursoCommandHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<bool> Handle(DeletarCursoCommand request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ObterPorIdAsync(request.Id);

            if (curso == null)
            {
                return false;
            }

            _cursoRepository.Remover(curso);

            return await _cursoRepository.SaveChangesAsync() > 0;
        }
    }
}

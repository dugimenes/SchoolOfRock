using Conteudo.Application.Dtos;
using Conteudo.Application.Queries;
using Conteudo.Infra.Repository;
using MediatR;

namespace Conteudo.Application.Handlers
{
    public class ObterCursoPorIdQueryHandler : IRequestHandler<ObterCursoPorIdQuery, CursoDto>
    {
        private readonly ICursoRepository _cursoRepository;

        public ObterCursoPorIdQueryHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<CursoDto> Handle(ObterCursoPorIdQuery request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ObterPorIdAsync(request.Id);

            if (curso == null)
            {
                return null;
            }

            var cursoDto = new CursoDto
            {
                Id = curso.Id,
                Nome = curso.Nome
                // ... outras propriedades
            };

            return cursoDto;
        }
    }
}

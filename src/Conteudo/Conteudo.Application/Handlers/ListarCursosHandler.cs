using Conteudo.Application.Dtos;
using Conteudo.Application.Queries;
using Conteudo.Infra.Repository;
using MediatR;

namespace Conteudo.Application.Handlers
{
    public class ListarCursosHandler : IRequestHandler<ListarCursosQuery, List<CursoDto>>
    {
        private readonly ICursoRepository _cursoRepository;

        public ListarCursosHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<List<CursoDto>> Handle(ListarCursosQuery request, CancellationToken cancellationToken)
        {
            var certificados = await _cursoRepository.ObterTodosAsync();

            return certificados.Select(c => new CursoDto
            {
                Id = c.Id,
                Nome = c.Nome,
                ConteudoProgramatico = c.ConteudoProgramatico.Descricao,
                Aulas = c.Aulas.Select(a => new AulaDto
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Conteudo = a.Conteudo,
                    MaterialUrl = a.MaterialUrl
                }).ToList()
            }).ToList();
        }
    }
}
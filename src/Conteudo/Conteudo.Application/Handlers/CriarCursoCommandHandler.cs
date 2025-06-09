using Conteudo.Application.Command;
using Conteudo.Domain.AggregateModel;
using Conteudo.Infra.Repository;
using MediatR;

namespace Conteudo.Application.Handlers
{
    public class CriarCursoCommandHandler : IRequestHandler<CriarCursoCommand, Guid>
    {
        private readonly ICursoRepository _cursoRepository;

        public CriarCursoCommandHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<Guid> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
        {
            var conteudoProgramatico = new ConteudoProgramatico(request.ConteudoProgramatico);
            var novoCurso = new Curso(request.Nome, conteudoProgramatico);

            await _cursoRepository.AdicionarAsync(novoCurso);
            await _cursoRepository.SaveChangesAsync();

            return novoCurso.Id;
        }
    }
}

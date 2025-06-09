using Conteudo.Application.Command;
using Conteudo.Domain.AggregateModel;
using Conteudo.Infra.Repository;
using MediatR;

namespace Conteudo.Application.Handlers
{
    public class AdicionarAulaCommandHandler : IRequestHandler<AdicionarAulaCommand, Guid>
    {
        private readonly ICursoRepository _cursoRepository;

        public AdicionarAulaCommandHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<Guid> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ObterPorIdAsync(request.CursoId);
            if (curso == null)
            {
                throw new Exception("Curso não encontrado.");
            }

            var novaAula = new Aula(request.Titulo, request.Conteudo, request.CursoId, request.MaterialUrl);

            curso.AdicionarAula(novaAula);

            _cursoRepository.Atualizar(curso);
            await _cursoRepository.SaveChangesAsync();

            return novaAula.Id;
        }
    }
}

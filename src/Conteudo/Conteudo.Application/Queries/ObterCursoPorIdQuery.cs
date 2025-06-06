using Conteudo.Application.Dtos;
using MediatR;

namespace Conteudo.Application.Queries
{
    public class ObterCursoPorIdQuery : IRequest<CursoDto>
    {
        public Guid Id { get; set; }

        public ObterCursoPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
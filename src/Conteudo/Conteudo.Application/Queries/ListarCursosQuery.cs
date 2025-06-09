using Conteudo.Application.Dtos;
using MediatR;

namespace Conteudo.Application.Queries
{
    public class ListarCursosQuery : IRequest<List<CursoDto>>
    {
    }
}

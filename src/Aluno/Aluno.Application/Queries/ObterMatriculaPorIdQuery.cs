using Aluno.Application.Dtos;
using MediatR;

namespace Conteudo.Application.Queries
{
    public class ObterMatriculaPorIdQuery : IRequest<MatriculaDto>
    {
        public Guid Id { get; set; }

        public ObterMatriculaPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
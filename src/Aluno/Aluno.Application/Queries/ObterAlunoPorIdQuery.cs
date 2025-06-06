using Aluno.Application.Dtos;
using MediatR;

namespace Conteudo.Application.Queries
{
    public class ObterAlunoPorIdQuery : IRequest<AlunoDto>
    {
        public Guid Id { get; set; }

        public ObterAlunoPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
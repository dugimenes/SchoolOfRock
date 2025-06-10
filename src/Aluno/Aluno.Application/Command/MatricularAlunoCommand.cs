using Aluno.Application.Dtos;
using MediatR;

namespace Aluno.Application.Command
{
    public class MatricularAlunoCommand : IRequest<Guid>
    {
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public DadosCartaoDto DadosCartao { get; set; }
    }
}
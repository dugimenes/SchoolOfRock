using Aluno.Application.Dtos;
using Aluno.Infra.Repository;
using Conteudo.Application.Queries;
using MediatR;

namespace Conteudo.Application.Handlers
{
    public class ObterAlunoPorIdQueryHandler : IRequestHandler<ObterAlunoPorIdQuery, AlunoDto>
    {
        private readonly IAlunoRepository _alunoRepository;

        public ObterAlunoPorIdQueryHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<AlunoDto> Handle(ObterAlunoPorIdQuery request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(request.Id);

            if (aluno == null)
            {
                return null;
            }

            var cursoDto = new AlunoDto
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email
                // ... outras propriedades
            };

            return cursoDto;
        }
    }
}
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

            // Mapeamento da entidade para o DTO
            var alunoDto = new AlunoDto
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Matriculas = aluno.Matriculas.Select(m => new MatriculaDto
                {
                    Id = m.Id,
                    CursoId = m.CursoId,
                    Status = m.Status,
                    DataMatricula = m.DataMatricula
                }).ToList()
            };

            return alunoDto;
        }
    }
}
using Aluno.Application.Dtos;
using Aluno.Infra.Repository;
using Conteudo.Application.Queries;
using MediatR;

namespace Conteudo.Application.Handlers
{
    public class ObterMatriculaPorIdQueryHandler : IRequestHandler<ObterMatriculaPorIdQuery, MatriculaDto>
    {
        private readonly IMatriculaRepository _matriculaRepository;

        public ObterMatriculaPorIdQueryHandler(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository;
        }

        public async Task<MatriculaDto> Handle(ObterMatriculaPorIdQuery request, CancellationToken cancellationToken)
        {
            var matricula = await _matriculaRepository.ObterPorIdAsync(request.Id);

            if (matricula == null)
            {
                return null;
            }

            var matriculaDto = new MatriculaDto
            {
                Id = matricula.Id,
                DataMatricula = matricula.DataMatricula,
                Status = matricula.Status

                // ... outras propriedades
            };

            return matriculaDto;
        }
    }
}
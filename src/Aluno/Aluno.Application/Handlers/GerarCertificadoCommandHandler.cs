using Aluno.Application.Command;
using Aluno.Application.Dtos;
using Aluno.Domain.AggregateModel;
using Aluno.Domain.Enumerations;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class GerarCertificadoCommandHandler : IRequestHandler<GerarCertificadoCommand, CertificadoDto>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly ICertificadoRepository _certificadoRepository;
        private readonly IMatriculaRepository _matriculaRepository;

        public GerarCertificadoCommandHandler(IAlunoRepository alunoRepository, ICertificadoRepository certificadoRepository, IMatriculaRepository matriculaRepository)
        {
            _alunoRepository = alunoRepository;
            _certificadoRepository = certificadoRepository;
            _matriculaRepository = matriculaRepository;
        }

        public async Task<CertificadoDto> Handle(GerarCertificadoCommand request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(request.AlunoId);
            if (aluno == null)
            {
                throw new Exception("Verifique o Id informado para o aluno.");
            }

            var certificadoExistente =
                await _certificadoRepository.ObterPorCursoEAlunoIdAsync(request.CursoId, request.AlunoId);

            if (certificadoExistente != null)
            {
                throw new Exception("O aluno já possui um certificado para este curso.");
            }

            var matricula = await _matriculaRepository.ObterPorAlunoECursoIdAsync(request.AlunoId, request.CursoId);

            if (matricula == null)
            {
                throw new Exception("O aluno não está matriculado neste curso.");
            }

            if (matricula.Status != StatusMatricula.Concluida)
            {
                throw new Exception("A matrícula do aluno neste curso não está concluída.");
            }

            var novoCertificado = new Certificado(request.CursoId, request.AlunoId);

            await _certificadoRepository.AdicionarAsync(novoCertificado);
            await _certificadoRepository.SaveChangesAsync();

            return new CertificadoDto
            {
                Id = novoCertificado.Id,
                AlunoId = novoCertificado.AlunoId,
                CursoId = novoCertificado.CursoId,
                DataEmissao = novoCertificado.DataEmissao
            };
        }
    }
}
using Aluno.Application.Dtos;
using Aluno.Application.Queries;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class ListarCertificadosPorAlunoQueryHandler : IRequestHandler<ListarCertificadosPorAlunoQuery, List<CertificadoDto>>
    {
        private readonly ICertificadoRepository _certificadoRepository;

        public ListarCertificadosPorAlunoQueryHandler(ICertificadoRepository certificadoRepository)
        {
            _certificadoRepository = certificadoRepository;
        }

        public async Task<List<CertificadoDto>> Handle(ListarCertificadosPorAlunoQuery request, CancellationToken cancellationToken)
        {
            var certificados = await _certificadoRepository.ObterPorAlunoIdAsync(request.AlunoId);

            return certificados.Select(c => new CertificadoDto
            {
                Id = c.Id,
                AlunoId = c.AlunoId,
                CursoId = c.CursoId,
                DataEmissao = c.DataEmissao
            }).ToList();
        }
    }
}

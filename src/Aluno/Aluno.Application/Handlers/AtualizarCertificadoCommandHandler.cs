using Aluno.Application.Command;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class AtualizarCertificadoCommandHandler : IRequestHandler<AtualizarCertificadoCommand, bool>
    {
        private readonly ICertificadoRepository _certificadoRepository;

        public AtualizarCertificadoCommandHandler(ICertificadoRepository certificadoRepository)
        {
            _certificadoRepository = certificadoRepository;
        }

        public async Task<bool> Handle(AtualizarCertificadoCommand request, CancellationToken cancellationToken)
        {
            var certificado = await _certificadoRepository.ObterPorIdAsync(request.CertificadoId);

            if (certificado == null)
            {
                return false;
            }

            certificado.AtualizarDataEmissao(request.NovaDataEmissao);

            _certificadoRepository.Atualizar(certificado);

            await _certificadoRepository.SaveChangesAsync();

            return true;
        }
    }
}

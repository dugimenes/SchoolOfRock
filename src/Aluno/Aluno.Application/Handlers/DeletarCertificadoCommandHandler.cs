using Aluno.Application.Command;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class DeletarCertificadoCommandHandler : IRequestHandler<DeletarCertificadoCommand, bool>
    {
        private readonly ICertificadoRepository _certificadoRepository;

        public DeletarCertificadoCommandHandler(ICertificadoRepository certificadoRepository)
        {
            _certificadoRepository = certificadoRepository;
        }

        public async Task<bool> Handle(DeletarCertificadoCommand request, CancellationToken cancellationToken)
        {
            var certificado = await _certificadoRepository.ObterPorIdAsync(request.CertificadoId);

            if (certificado == null)
            {
                return false;
            }

            _certificadoRepository.Remover(certificado);

            return await _certificadoRepository.SaveChangesAsync() > 0;
        }
    }
}
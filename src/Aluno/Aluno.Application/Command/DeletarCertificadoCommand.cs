using MediatR;

namespace Aluno.Application.Command
{
    public class DeletarCertificadoCommand : IRequest<bool>
    {
        public Guid CertificadoId { get; set; }

        public DeletarCertificadoCommand(Guid certificadoId)
        {
            CertificadoId = certificadoId;
        }
    }
}

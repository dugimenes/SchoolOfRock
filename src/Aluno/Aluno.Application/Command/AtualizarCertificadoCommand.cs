using MediatR;

namespace Aluno.Application.Command
{
    public class AtualizarCertificadoCommand : IRequest<bool>
    {
        public Guid CertificadoId { get; set; }
        public DateTime NovaDataEmissao { get; set; }
    }
}

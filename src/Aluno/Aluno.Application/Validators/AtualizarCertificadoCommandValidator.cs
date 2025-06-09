using Aluno.Application.Command;
using FluentValidation;

namespace Aluno.Application.Validators
{
    public class AtualizarCertificadoCommandValidator : AbstractValidator<AtualizarCertificadoCommand>
    {
        public AtualizarCertificadoCommandValidator()
        {
            RuleFor(x => x.CertificadoId)
                .NotEmpty()
                .WithMessage("O ID do Certificado é obrigatório.");

            RuleFor(x => x.NovaDataEmissao)
                .NotEmpty()
                .WithMessage("A nova data de emissão é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("A data de emissão não pode ser uma data futura.");
        }
    }
}

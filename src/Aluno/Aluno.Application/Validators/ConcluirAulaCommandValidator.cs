using Aluno.Application.Command;
using FluentValidation;

namespace Aluno.Application.Validators
{
    public class ConcluirAulaCommandValidator : AbstractValidator<ConcluirAulaCommand>
    {
        public ConcluirAulaCommandValidator()
        {
            RuleFor(x => x.AlunoId)
                .NotEmpty()
                .WithMessage("O ID do Aluno é obrigatório.");

            RuleFor(x => x.AulaId)
                .NotEmpty()
                .WithMessage("O ID da Aula é obrigatório.");
        }
    }
}

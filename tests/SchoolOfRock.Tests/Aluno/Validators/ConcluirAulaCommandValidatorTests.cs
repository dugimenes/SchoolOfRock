using Aluno.Application.Command;
using FluentValidation;

namespace SchoolOfRock.Tests.Aluno.Validators
{
    public class ConcluirAulaCommandValidatorTests : AbstractValidator<ConcluirAulaCommand>
    {
        public ConcluirAulaCommandValidatorTests()
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

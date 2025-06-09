using FluentValidation;
using Identity.Application.Command;

namespace SchoolOfRock.Tests.Identity.Validators
{
    public class RegisterCommandValidatorTests : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidatorTests()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.");

            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email fornecido não é válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
        }
    }
}

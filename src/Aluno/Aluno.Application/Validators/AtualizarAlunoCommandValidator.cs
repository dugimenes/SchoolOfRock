using Aluno.Application.Command;
using FluentValidation;

namespace Aluno.Application.Validators
{
    public class AtualizarAlunoCommandValidator : AbstractValidator<AtualizarAlunoCommand>
    {
        public AtualizarAlunoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do aluno é obrigatório.");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O formato do e-mail é inválido.")
                .MaximumLength(150).WithMessage("O e-mail não pode exceder 150 caracteres.");
        }
    }
}
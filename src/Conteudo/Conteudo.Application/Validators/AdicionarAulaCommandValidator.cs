using Conteudo.Application.Command;
using FluentValidation;

namespace Conteudo.Application.Validators
{
    public class AdicionarAulaCommandValidator : AbstractValidator<AdicionarAulaCommand>
    {
        public AdicionarAulaCommandValidator()
        {
            RuleFor(x => x.CursoId)
                .NotEmpty()
                .WithMessage("O ID do Curso é obrigatório.");

            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("O título da aula é obrigatório.")
                .MaximumLength(200).WithMessage("O título da aula não pode exceder 200 caracteres.");

            RuleFor(x => x.Conteudo)
                .NotEmpty().WithMessage("O conteúdo da aula é obrigatório.")
                .MaximumLength(4000).WithMessage("O conteúdo da aula não pode exceder 4000 caracteres.");

            RuleFor(x => x.MaterialUrl)
                .MaximumLength(500).WithMessage("A URL do material não pode exceder 500 caracteres.");
        }
    }
}

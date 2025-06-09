using Conteudo.Application.Command;
using FluentValidation;

namespace Conteudo.Application.Validators
{
    public class CriarCursoCommandValidator : AbstractValidator<CriarCursoCommand>
    {
        public CriarCursoCommandValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do curso é obrigatório.")
                .MaximumLength(200).WithMessage("O nome do curso não pode exceder 200 caracteres.");

            RuleFor(x => x.ConteudoProgramatico)
                .NotEmpty().WithMessage("O conteúdo programático é obrigatório.")
                .MaximumLength(1000).WithMessage("O conteúdo programático não pode exceder 1000 caracteres.");
        }
    }
}
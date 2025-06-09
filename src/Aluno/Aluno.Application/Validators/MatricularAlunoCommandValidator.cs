using Aluno.Application.Command;
using FluentValidation;

namespace Aluno.Application.Validators
{
    public class MatricularAlunoCommandValidator : AbstractValidator<MatricularAlunoCommand>
    {
        public MatricularAlunoCommandValidator()
        {
            RuleFor(x => x.AlunoId)
                .NotEmpty()
                .WithMessage("O ID do Aluno é obrigatório.");

            RuleFor(x => x.CursoId)
                .NotEmpty()
                .WithMessage("O ID do Curso é obrigatório.");
        }
    }
}

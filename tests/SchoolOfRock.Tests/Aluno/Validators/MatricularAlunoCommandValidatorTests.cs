using Aluno.Application.Command;
using FluentValidation;

namespace SchoolOfRock.Tests.Aluno.Validators
{
    public class MatricularAlunoCommandValidatorTests : AbstractValidator<MatricularAlunoCommand>
    {
        public MatricularAlunoCommandValidatorTests()
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

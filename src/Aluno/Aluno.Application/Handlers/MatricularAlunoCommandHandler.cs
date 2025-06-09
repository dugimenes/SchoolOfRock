using Aluno.Application.Command;
using Aluno.Domain.AggregateModel;
using Aluno.Infra.Repository;
using MediatR;
using SchoolOfRock.Contracts.Aluno;

namespace Aluno.Application.Handlers
{
    public class MatricularAlunoCommandHandler : IRequestHandler<MatricularAlunoCommand, Guid>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMediator _mediator;

        public MatricularAlunoCommandHandler(IAlunoRepository alunoRepository, IMediator mediator)
        {
            _alunoRepository = alunoRepository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(MatricularAlunoCommand request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(request.AlunoId);
            if (aluno == null)
            {
                throw new Exception("Aluno não encontrado.");
            }

            var novaMatricula = new Matricula(request.CursoId, request.AlunoId);

            aluno.AdicionarMatricula(novaMatricula);

            _alunoRepository.Atualizar(aluno);
            await _alunoRepository.SaveChangesAsync();

            await _mediator.Publish(new AlunoMatriculadoEvent(novaMatricula.Id, aluno.Id, request.CursoId), cancellationToken);

            return novaMatricula.Id;
        }
    }
}

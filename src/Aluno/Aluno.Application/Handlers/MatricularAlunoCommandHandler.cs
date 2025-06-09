using Aluno.Application.Command;
using Aluno.Domain.AggregateModel;
using Aluno.Infra.Repository;
using MediatR;

namespace Aluno.Application.Handlers
{
    public class MatricularAlunoCommandHandler : IRequestHandler<MatricularAlunoCommand, Guid>
    {
        private readonly IAlunoRepository _alunoRepository;

        public MatricularAlunoCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
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

            // Aqui você publicaria um evento 'AlunoMatriculadoEvent' para o PagamentoContext iniciar a cobrança.

            return novaMatricula.Id;
        }
    }
}

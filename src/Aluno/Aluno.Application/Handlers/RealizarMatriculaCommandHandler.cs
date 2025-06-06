using Aluno.Application.Command;
using Aluno.Domain.AggregateModel;
using Aluno.Infra.Repository;
using MediatR;

public class RealizarMatriculaCommandHandler : IRequestHandler<RealizarMatriculaCommand, Guid>
{
    private readonly IAlunoRepository _alunoRepository; // Repositório específico do contexto

    public RealizarMatriculaCommandHandler(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<Guid> Handle(RealizarMatriculaCommand request, CancellationToken cancellationToken)
    {
        // 1. Orquestra a regra de negócio
        var aluno = await _alunoRepository.ObterPorIdAsync(request.AlunoId);
        // (Validações, ex: se o aluno existe, se o curso existe - pode chamar outro serviço/repositório)

        var novaMatricula = new Matricula(request.CursoId, request.AlunoId);
        aluno.AdicionarMatricula(novaMatricula);

        // 2. Persiste a alteração
        _alunoRepository.Atualizar(aluno);

        // 3. (Opcional) Dispara um evento de domínio (ver Passo 5)

        return novaMatricula.Id;
    }
}
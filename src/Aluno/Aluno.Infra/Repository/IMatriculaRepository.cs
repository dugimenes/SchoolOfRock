using Aluno.Domain.AggregateModel;

namespace Aluno.Infra.Repository
{
    public interface IMatriculaRepository
    {
        Task<Matricula> ObterPorIdAsync(Guid id);

        Task<IEnumerable<Matricula>> ObterTodosAsync();

        Task AdicionarAsync(Matricula matricula);

        void Atualizar(Matricula matricula);

        void Remover(Matricula matricula);

        Task<int> SaveChangesAsync();

        Task<IEnumerable<Matricula>> ObterPorAlunoIdAsync(Guid alunoId);

        Task<IEnumerable<Matricula>> ObterPorCursoIdAsync(Guid cursoId);
        Task<Matricula> ObterPorAlunoECursoIdAsync(Guid alunoId, Guid cursoId);
    }
}
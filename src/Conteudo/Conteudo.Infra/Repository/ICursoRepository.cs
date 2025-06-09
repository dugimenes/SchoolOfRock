using Conteudo.Domain.AggregateModel;

namespace Conteudo.Infra.Repository
{
    public interface ICursoRepository
    {
        Task<Curso> ObterPorIdAsync(Guid id);

        Task<IEnumerable<Curso>> ObterTodosAsync();

        Task AdicionarAsync(Curso curso);

        void Atualizar(Curso curso);

        void Remover(Curso curso);

        Task<int> SaveChangesAsync();
    }
}

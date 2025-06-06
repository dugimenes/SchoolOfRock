namespace Aluno.Infra.Repository
{
    public interface IAlunoRepository
    {
        Task<Domain.AggregateModel.Aluno> ObterPorIdAsync(Guid id);

        Task<IEnumerable<Domain.AggregateModel.Aluno>> ObterTodosAsync();

        Task AdicionarAsync(Domain.AggregateModel.Aluno aluno);

        void Atualizar(Domain.AggregateModel.Aluno aluno);

        void Remover(Domain.AggregateModel.Aluno aluno);

        Task<int> SaveChangesAsync();
    }
}

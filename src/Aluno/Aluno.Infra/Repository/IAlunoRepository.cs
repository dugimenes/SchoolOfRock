using SchoolOfRock.Shared.Repository;

namespace Aluno.Infra.Repository
{
    public interface IAlunoRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<Domain.AggregateModel.Aluno> ObterPorIdAsync(Guid id);

        Task<IEnumerable<Domain.AggregateModel.Aluno>> ObterTodosAsync();

        Task AdicionarAsync(Domain.AggregateModel.Aluno aluno);

        void Adicionar(Domain.AggregateModel.Aluno aluno);

        void Atualizar(Domain.AggregateModel.Aluno aluno);

        void Remover(Domain.AggregateModel.Aluno aluno);

        Task<int> SaveChangesAsync();
    }
}

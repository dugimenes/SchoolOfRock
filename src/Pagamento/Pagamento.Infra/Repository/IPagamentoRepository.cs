namespace Pagamento.Infra.Repository
{
    public interface IPagamentoRepository
    {
    Task<Domain.AggregateModel.Pagamento> ObterPorIdAsync(Guid id);

    Task<Domain.AggregateModel.Pagamento> ObterPagamentoMatriculaPendenteAsync(Guid id);

    Task<IEnumerable<Domain.AggregateModel.Pagamento>> ObterTodosAsync();

    Task AdicionarAsync(Domain.AggregateModel.Pagamento pagamento);

    void Atualizar(Domain.AggregateModel.Pagamento pagamento);

    void Remover(Domain.AggregateModel.Pagamento pagamento);

    Task<int> SaveChangesAsync();

    Task<IEnumerable<Domain.AggregateModel.Pagamento>> ObterPorMatriculaIdAsync(Guid matriculaId);
    }
}

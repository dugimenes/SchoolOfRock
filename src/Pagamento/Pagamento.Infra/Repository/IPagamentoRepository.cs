namespace Pagamento.Infra.Repository
{
    public interface IPagamentoRepository
    {
    /// <summary>
    /// Obtém um pagamento pelo seu ID (GUID).
    /// </summary>
    /// <param name="id">Identificador único do Pagamento.</param>
    /// <returns>O Pagamento correspondente ou null se não existir.</returns>
    Task<Domain.AggregateModel.Pagamento> ObterPorIdAsync(Guid id);

    /// <summary>
    /// Obtém todos os pagamentos cadastrados.
    /// </summary>
    /// <returns>Lista de todos os pagamentos.</returns>
    Task<IEnumerable<Domain.AggregateModel.Pagamento>> ObterTodosAsync();

    /// <summary>
    /// Insere um novo pagamento no repositório.
    /// </summary>
    /// <param name="pagamento">Objeto Pagamento a ser adicionado.</param>
    Task AdicionarAsync(Domain.AggregateModel.Pagamento pagamento);

    /// <summary>
    /// Atualiza os dados de um pagamento existente.
    /// </summary>
    /// <param name="pagamento">Objeto Pagamento com os dados atualizados.</param>
    void Atualizar(Domain.AggregateModel.Pagamento pagamento);

    /// <summary>
    /// Remove um pagamento do repositório.
    /// </summary>
    /// <param name="pagamento">Objeto Pagamento a ser removido.</param>
    void Remover(Domain.AggregateModel.Pagamento pagamento);

    /// <summary>
    /// Persiste as alterações pendentes no contexto de dados.
    /// </summary>
    /// <returns>Número de registros afetados.</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Obtém todos os pagamentos vinculados a uma determinada matrícula.
    /// </summary>
    /// <param name="matriculaId">ID da Matrícula.</param>
    /// <returns>Lista de Pagamentos daquela matrícula.</returns>
    Task<IEnumerable<Domain.AggregateModel.Pagamento>> ObterPorMatriculaIdAsync(Guid matriculaId);
}

}

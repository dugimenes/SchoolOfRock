using Conteudo.Domain.AggregateModel;

namespace Conteudo.Infra.Repository
{
    public interface ICursoRepository
    {
        /// <summary>
        /// Obtém um curso pelo seu ID (GUID).
        /// </summary>
        /// <param name="id">Identificador único do Curso.</param>
        /// <returns>O Curso correspondente ou null se não existir.</returns>
        Task<Curso> ObterPorIdAsync(Guid id);

        /// <summary>
        /// Obtém todos os cursos cadastrados.
        /// </summary>
        /// <returns>Lista de todos os cursos.</returns>
        Task<IEnumerable<Curso>> ObterTodosAsync();

        /// <summary>
        /// Insere um novo curso no repositório.
        /// </summary>
        /// <param name="curso">Objeto Curso a ser adicionado.</param>
        Task AdicionarAsync(Curso curso);

        /// <summary>
        /// Atualiza os dados de um curso existente.
        /// </summary>
        /// <param name="curso">Objeto Curso com os dados atualizados.</param>
        void Atualizar(Curso curso);

        /// <summary>
        /// Remove um curso do repositório.
        /// </summary>
        /// <param name="curso">Objeto Curso a ser removido.</param>
        void Remover(Curso curso);

        /// <summary>
        /// Persiste as alterações pendentes no contexto de dados.
        /// </summary>
        /// <returns>Número de registros afetados.</returns>
        Task<int> SaveChangesAsync();
    }
}

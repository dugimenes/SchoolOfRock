using Microsoft.EntityFrameworkCore;

namespace Pagamento.Infra.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PagamentoDbContext _context;
        private readonly DbSet<Domain.AggregateModel.Pagamento> _dbSet;

        public PagamentoRepository(PagamentoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<Domain.AggregateModel.Pagamento>();
        }

        /// <inheritdoc />
        public async Task<Domain.AggregateModel.Pagamento> ObterPorIdAsync(Guid id)
        {
            // Busca direta pelo identificador primário
            return await _dbSet.FindAsync(id);

            // Se precisar incluir navegação a Matricula, descomente e ajuste:
            // return await _dbSet
            //     .Include(p => p.Matricula)
            //     .SingleOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Domain.AggregateModel.Pagamento>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <inheritdoc />
        public async Task AdicionarAsync(Domain.AggregateModel.Pagamento pagamento)
        {
            if (pagamento == null)
                throw new ArgumentNullException(nameof(pagamento));

            await _dbSet.AddAsync(pagamento);
        }

        /// <inheritdoc />
        public void Atualizar(Domain.AggregateModel.Pagamento pagamento)
        {
            if (pagamento == null)
                throw new ArgumentNullException(nameof(pagamento));

            _dbSet.Update(pagamento);
        }

        /// <inheritdoc />
        public void Remover(Domain.AggregateModel.Pagamento pagamento)
        {
            if (pagamento == null)
                throw new ArgumentNullException(nameof(pagamento));

            _dbSet.Remove(pagamento);
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Domain.AggregateModel.Pagamento>> ObterPorMatriculaIdAsync(Guid matriculaId)
        {
            return await _dbSet
                .Where(p => p.MatriculaId == matriculaId)
                .ToListAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Pagamento.Domain.AggregateModel;

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

        public async Task<Domain.AggregateModel.Pagamento> ObterPorIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);

        }

        public async Task<Domain.AggregateModel.Pagamento> ObterPagamentoMatriculaPendenteAsync(Guid matriculaId)
        {
            return await _dbSet
                .SingleOrDefaultAsync(a => a.MatriculaId == matriculaId && a.StatusPagamento == StatusPagamento.Pendente);
        }

        public async Task<IEnumerable<Domain.AggregateModel.Pagamento>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AdicionarAsync(Domain.AggregateModel.Pagamento pagamento)
        {
            if (pagamento == null)
                throw new ArgumentNullException(nameof(pagamento));

            await _dbSet.AddAsync(pagamento);
        }

        public void Atualizar(Domain.AggregateModel.Pagamento pagamento)
        {
            if (pagamento == null)
                throw new ArgumentNullException(nameof(pagamento));

            _dbSet.Update(pagamento);
        }

        public void Remover(Domain.AggregateModel.Pagamento pagamento)
        {
            if (pagamento == null)
                throw new ArgumentNullException(nameof(pagamento));

            _dbSet.Remove(pagamento);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Domain.AggregateModel.Pagamento>> ObterPorMatriculaIdAsync(Guid matriculaId)
        {
            return await _dbSet
                .Where(p => p.MatriculaId == matriculaId)
                .ToListAsync();
        }
    }
}
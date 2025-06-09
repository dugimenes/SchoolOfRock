using Conteudo.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.Infra.Repository
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ConteudoDbContext _context;
        private readonly DbSet<Curso> _dbSet;

        public CursoRepository(ConteudoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<Curso>();
        }

        public async Task<Curso> ObterPorIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Curso>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AdicionarAsync(Curso curso)
        {
            if (curso == null)
                throw new ArgumentNullException(nameof(curso));

            await _dbSet.AddAsync(curso);
        }

        public void Atualizar(Curso curso)
        {
            if (curso == null)
                throw new ArgumentNullException(nameof(curso));

            _dbSet.Update(curso);
        }

        public void Remover(Curso curso)
        {
            if (curso == null)
                throw new ArgumentNullException(nameof(curso));

            _dbSet.Remove(curso);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

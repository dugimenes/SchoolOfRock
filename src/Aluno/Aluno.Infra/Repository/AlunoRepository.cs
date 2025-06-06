using Microsoft.EntityFrameworkCore;

namespace Aluno.Infra.Repository
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AlunoDbContext _context;
        private readonly DbSet<Domain.AggregateModel.Aluno> _dbSet;

        public AlunoRepository(AlunoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<Domain.AggregateModel.Aluno>();
        }

        public async Task<Domain.AggregateModel.Aluno> ObterPorIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
            // return await _dbSet
            //     .Include(a => a.Matriculas)
            //     .Include(a => a.Certificados)
            //     .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Domain.AggregateModel.Aluno>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AdicionarAsync(Domain.AggregateModel.Aluno aluno)
        {
            if (aluno == null)
                throw new ArgumentNullException(nameof(aluno));

            await _dbSet.AddAsync(aluno);
        }

        public void Atualizar(Domain.AggregateModel.Aluno aluno)
        {
            if (aluno == null)
                throw new ArgumentNullException(nameof(aluno));

            _dbSet.Update(aluno);
        }

        public void Remover(Domain.AggregateModel.Aluno aluno)
        {
            if (aluno == null)
                throw new ArgumentNullException(nameof(aluno));

            _dbSet.Remove(aluno);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

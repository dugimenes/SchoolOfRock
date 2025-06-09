using Aluno.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;

namespace Aluno.Infra.Repository
{
    public class CertificadoRepository : ICertificadoRepository
    {
        private readonly AlunoDbContext _context;
        private readonly DbSet<Certificado> _dbSet;

        public CertificadoRepository(AlunoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<Certificado>();
        }

        public async Task<Certificado> ObterPorIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Certificado>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AdicionarAsync(Certificado certificado)
        {
            if (certificado == null)
                throw new ArgumentNullException(nameof(certificado));

            await _dbSet.AddAsync(certificado);
        }

        public void Atualizar(Certificado certificado)
        {
            if (certificado == null)
                throw new ArgumentNullException(nameof(certificado));

            _dbSet.Update(certificado);
        }

        public void Remover(Certificado certificado)
        {
            if (certificado == null)
                throw new ArgumentNullException(nameof(certificado));

            _dbSet.Remove(certificado);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Certificado>> ObterPorAlunoIdAsync(Guid alunoId)
        {
            return await _dbSet
                .Where(c => c.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Certificado>> ObterPorCursoIdAsync(Guid cursoId)
        {
            return await _dbSet
                .Where(c => c.CursoId == cursoId)
                .ToListAsync();
        }

        public async Task<Certificado> ObterPorCursoEAlunoIdAsync(Guid alunoId, Guid cursoId)
        {
            return await _dbSet
                .SingleOrDefaultAsync(c => c.CursoId == cursoId
                                           && c.AlunoId == alunoId);
        }
    }
}
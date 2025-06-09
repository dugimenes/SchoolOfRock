using Aluno.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;

namespace Aluno.Infra.Repository
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly AlunoDbContext _context;
        private readonly DbSet<Matricula> _dbSet;

        public MatriculaRepository(AlunoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<Matricula>();
        }

        public async Task<Matricula> ObterPorIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
            // return await _dbSet
            //     .Include(m => m.Aluno)
            //     .Include(m => m.Curso)
            //     .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Matricula>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AdicionarAsync(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentNullException(nameof(matricula));

            await _dbSet.AddAsync(matricula);
        }

        public void Atualizar(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentNullException(nameof(matricula));

            _dbSet.Update(matricula);
        }

        public void Remover(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentNullException(nameof(matricula));

            _dbSet.Remove(matricula);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Matricula>> ObterPorAlunoIdAsync(Guid alunoId)
        {
            return await _dbSet
                .Where(m => m.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task<Matricula> ObterPorAlunoECursoIdAsync(Guid alunoId, Guid cursoId)
        {
            return await _dbSet
                .SingleOrDefaultAsync(m => m.AlunoId == alunoId 
                                      && m.CursoId == cursoId);
        }

        public async Task<IEnumerable<Matricula>> ObterPorCursoIdAsync(Guid cursoId)
        {
            return await _dbSet
                .Where(m => m.CursoId == cursoId)
                .ToListAsync();
        }
    }
}
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

        /// <inheritdoc />
        public async Task<Curso> ObterPorIdAsync(Guid id)
        {
            // Busca direta pelo identificador primário.
            // Se precisar carregar Aulas ou ConteudoProgramatico, use Include()
            return await _dbSet.FindAsync(id);

            // Exemplo com Include (caso ConteudoProgramatico ou Aulas sejam propriedades de navegação):
            // return await _dbSet
            //     .Include(c => c.Aulas)
            //     .SingleOrDefaultAsync(c => c.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Curso>> ObterTodosAsync()
        {
            // Retorna todos os cursos.
            return await _dbSet.ToListAsync();
        }

        /// <inheritdoc />
        public async Task AdicionarAsync(Curso curso)
        {
            if (curso == null)
                throw new ArgumentNullException(nameof(curso));

            await _dbSet.AddAsync(curso);
        }

        /// <inheritdoc />
        public void Atualizar(Curso curso)
        {
            if (curso == null)
                throw new ArgumentNullException(nameof(curso));

            _dbSet.Update(curso);
        }

        /// <inheritdoc />
        public void Remover(Curso curso)
        {
            if (curso == null)
                throw new ArgumentNullException(nameof(curso));

            _dbSet.Remove(curso);
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

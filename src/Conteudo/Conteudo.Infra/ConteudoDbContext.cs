using Conteudo.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.Infra
{
    public class ConteudoDbContext : DbContext
    {
        public ConteudoDbContext(DbContextOptions<ConteudoDbContext> options) : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ConteudoDbContext).Assembly);
        }
        
    }
}
using Microsoft.EntityFrameworkCore;

namespace Pagamento.Infra
{
    public class PagamentoDbContext : DbContext
    {
        public PagamentoDbContext(DbContextOptions<PagamentoDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.AggregateModel.Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(PagamentoDbContext).Assembly);
        }
        
    }
}
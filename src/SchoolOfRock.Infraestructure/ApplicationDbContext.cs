using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCF.Core.Identity;
using SchoolOfRock.Data.Configuration;
using SchoolOfRock.Domain.Common;
using SchoolOfRock.Domain.Entity;

namespace SchoolOfRock.Infraestructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Entity).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(ApplicationUser))
                {
                    var method = typeof(ModelBuilder).GetMethod(nameof(ModelBuilder.ApplyConfiguration), new[] { typeof(IEntityTypeConfiguration<>) });
                    var genericMethod = method.MakeGenericMethod(entityType.ClrType);
                    genericMethod.Invoke(modelBuilder, new[] { Activator.CreateInstance(typeof(EntityConfiguration<>).MakeGenericType(entityType.ClrType)) });
                }
            }
        }
    }
}
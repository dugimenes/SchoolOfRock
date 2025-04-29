using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolOfRock.Domain.Entity;

namespace SchoolOfRock.Infraestructure.Configuration
{
    public class AlunoEntityConfiguration : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasMany(a => a.Matriculas)
                .WithOne()
                .HasForeignKey(m => m.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Certificados)
                .WithOne()
                .HasForeignKey(c => c.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(a => a.DadosCartao, navigationBuilder =>
            {
                navigationBuilder.Property(dc => dc.Numero).HasColumnName("CartaoNumero");
                navigationBuilder.Property(dc => dc.NomeTitular).HasColumnName("CartaoNomeTitular");
                navigationBuilder.Property(dc => dc.Expiracao).HasColumnName("CartaoExpiracao");
                navigationBuilder.Property(dc => dc.Cvv).HasColumnName("CartaoCvv");
            });

            builder.OwnsOne(a => a.HistoricoAprendizado, historico =>
            {
                historico.OwnsMany(h => h.AulasConcluidas, aulas =>
                {
                    aulas.WithOwner().HasForeignKey("AlunoId");

                    aulas.Property<Guid>(a => a.AulaId)
                        .HasColumnName("AulaId")
                        .IsRequired();

                    aulas.Property<DateTime>(a => a.DataConclusao)
                        .HasColumnName("DataConclusao")
                        .IsRequired();

                    aulas.HasKey("AulaId", "AlunoId"); // Composite Key!
                });
            });
        }
    }
}
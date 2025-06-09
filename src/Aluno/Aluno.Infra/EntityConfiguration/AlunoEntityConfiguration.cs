using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aluno.Infra.EntityConfiguration
{
    public class AlunoEntityConfiguration : IEntityTypeConfiguration<Domain.AggregateModel.Aluno>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregateModel.Aluno> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasMany(a => a.Matriculas)
                .WithOne(m => m.Aluno)
                .HasForeignKey(m => m.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Certificados)
                .WithOne(m => m.Aluno)
                .HasForeignKey(c => c.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(a => a.DadosCartao, navigationBuilder =>
            {
                navigationBuilder
                    .Property(dc => dc.Numero)
                    .HasColumnName("CartaoNumero")
                    .HasMaxLength(100)
                    .IsRequired(false);

                navigationBuilder
                    .Property(dc => dc.NomeTitular)
                    .HasColumnName("CartaoNomeTitular")
                    .HasMaxLength(100)
                    .IsRequired(false);

                navigationBuilder
                    .Property(dc => dc.Expiracao)
                    .HasColumnName("CartaoExpiracao")
                    .IsRequired(false);

                navigationBuilder
                    .Property(dc => dc.Cvv)
                    .HasColumnName("CartaoCvv")
                    .IsRequired(false);
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

                    aulas.HasKey("AulaId", "AlunoId");
                });
            });
        }
    }
}
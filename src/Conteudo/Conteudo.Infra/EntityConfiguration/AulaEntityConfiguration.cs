using Conteudo.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conteudo.Infra.EntityConfiguration
{
    public class AulaEntityConfiguration : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Conteudo)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(a => a.MaterialUrl)
                .HasMaxLength(500);

            builder.HasOne(a => a.Curso)
                .WithMany(c => c.Aulas)
                .HasForeignKey(a => a.CursoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
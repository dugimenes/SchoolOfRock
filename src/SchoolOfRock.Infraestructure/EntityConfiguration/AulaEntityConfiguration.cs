using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SchoolOfRock.Domain.Entity;

namespace SchoolOfRock.Infraestructure.Configuration
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
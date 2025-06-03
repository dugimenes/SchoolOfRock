using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aluno.Infra.EntityConfiguration
{
    public class MatriculaEntityConfiguration : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.DataMatricula)
                .IsRequired();

            builder.Property(m => m.Status)
                .IsRequired();

            builder.HasIndex(m => new { m.CursoId, m.AlunoId })
                .IsUnique();
        }
    }
}

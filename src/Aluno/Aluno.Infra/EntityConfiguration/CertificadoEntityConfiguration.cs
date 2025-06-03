using Aluno.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aluno.Infra.EntityConfiguration
{
    public class CertificadoEntityConfiguration : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.DataEmissao)
                .IsRequired();

            builder.HasIndex(c => new { c.CursoId, c.AlunoId })
                .IsUnique();
        }
    }
}
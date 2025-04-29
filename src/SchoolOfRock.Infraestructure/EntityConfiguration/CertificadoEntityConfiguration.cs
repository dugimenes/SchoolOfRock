using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolOfRock.Domain.Entity;

namespace SchoolOfRock.Infraestructure.Configuration
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
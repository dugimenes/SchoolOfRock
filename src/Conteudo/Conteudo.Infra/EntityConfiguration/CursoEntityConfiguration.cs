using Conteudo.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conteudo.Infra.EntityConfiguration
{
    public class CursoEntityConfiguration : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.OwnsOne(c => c.ConteudoProgramatico, cp =>
            {
                cp.Property(p => p.Descricao)
                    .HasColumnName("ConteudoProgramaticoDescricao")
                    .HasMaxLength(1000)
                    .IsRequired();
            });

            builder.HasMany(c => c.Aulas)
                .WithOne(a => a.Curso)
                .HasForeignKey(a => a.CursoId);
        }
    }
}
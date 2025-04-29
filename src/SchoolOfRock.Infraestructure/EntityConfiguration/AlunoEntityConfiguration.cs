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
        }
    }
}
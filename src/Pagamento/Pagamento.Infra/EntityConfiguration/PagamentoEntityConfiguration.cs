﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pagamento.Infra.EntityConfiguration
{
    public class PagamentoEntityConfiguration : IEntityTypeConfiguration<Domain.AggregateModel.Pagamento>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregateModel.Pagamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.StatusPagamento)
                .IsRequired();

            //builder.OwnsOne(p => p.DadosCartao, dc =>
            //{
            //    dc.Property(d => d.Numero)
            //        .IsRequired()
            //        .HasMaxLength(16)
            //        .HasColumnName("CartaoNumero");

            //    dc.Property(d => d.NomeTitular)
            //        .IsRequired()
            //        .HasMaxLength(150)
            //        .HasColumnName("CartaoNomeTitular");

            //    dc.Property(d => d.Expiracao)
            //        .IsRequired()
            //        .HasMaxLength(7)
            //        .HasColumnName("CartaoExpiracao");

            //    dc.Property(d => d.Cvv)
            //        .IsRequired()
            //        .HasMaxLength(4)
            //        .HasColumnName("CartaoCvv");
            //});

            builder.Property(p => p.MatriculaId)
                .IsRequired();
        }
    }
}
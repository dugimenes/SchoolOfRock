﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pagamento.Infra;

#nullable disable

namespace SchoolOfRock.Data.Migrations.Pagamento
{
    [DbContext(typeof(PagamentoDbContext))]
    [Migration("20250604021430_InitialPagamentoMigration")]
    partial class InitialPagamentoMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.4");

            modelBuilder.Entity("Pagamento.Domain.AggregateModel.Pagamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MatriculaId")
                        .HasColumnType("TEXT");

                    b.Property<int>("StatusPagamento")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Pagamentos");
                });
#pragma warning restore 612, 618
        }
    }
}

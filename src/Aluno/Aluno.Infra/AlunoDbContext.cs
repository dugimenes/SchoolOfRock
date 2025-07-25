﻿using Aluno.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;
using SchoolOfRock.Shared.Repository;

namespace Aluno.Infra
{
    public class AlunoDbContext : DbContext, IUnitOfWork
    {
        public AlunoDbContext(DbContextOptions<AlunoDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.AggregateModel.Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AlunoDbContext).Assembly);
        }
        
    }
}
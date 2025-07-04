﻿using SchoolOfRock.Shared;

namespace Conteudo.Domain.AggregateModel
{
    public class Curso : Entity
    {
        public Guid Id { get; set; }
        public string Nome { get; private set; }
        public ConteudoProgramatico ConteudoProgramatico { get; private set; }
        public ICollection<Aula> Aulas { get; private set; } = new List<Aula>();

        protected Curso() { }

        public Curso(Guid id,string nome, ConteudoProgramatico conteudoProgramatico)
        {
            Id = id;
            Nome = nome;
            ConteudoProgramatico = conteudoProgramatico;
        }

        public Curso(string nome, ConteudoProgramatico conteudoProgramatico)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            ConteudoProgramatico = conteudoProgramatico;
        }

        public void AdicionarAula(Aula aula)
        {
            Aulas.Add(aula);
        }
    }
}
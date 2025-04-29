using SchoolOfRock.Domain.ValueObjects;

namespace SchoolOfRock.Domain.Entity
{
    public class Curso : Common.Entity
    {
        public string Nome { get; private set; }
        public ConteudoProgramatico ConteudoProgramatico { get; private set; }
        public ICollection<Aula> Aulas { get; private set; } = new List<Aula>();

        protected Curso() { }

        public Curso(string nome, ConteudoProgramatico conteudoProgramatico)
        {
            Nome = nome;
            ConteudoProgramatico = conteudoProgramatico;
        }

        public void AdicionarAula(Aula aula)
        {
            Aulas.Add(aula);
        }
    }
}

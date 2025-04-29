namespace SchoolOfRock.Domain.Entity
{
    public class Aluno : Common.Entity
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public ICollection<Matricula> Matriculas { get; private set; } = new List<Matricula>();
        public ICollection<Certificado> Certificados { get; private set; } = new List<Certificado>();

        protected Aluno() { }

        public Aluno(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }
    }
}

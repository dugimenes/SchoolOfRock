using SchoolOfRock.Shared;

namespace Aluno.Domain.AggregateModel
{
    public class Aluno : Entity
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public DadosCartao DadosCartao { get; private set; }
        public HistoricoAprendizado HistoricoAprendizado { get; private set; } = new();

        public ICollection<Matricula> Matriculas { get; private set; } = new List<Matricula>();
        public ICollection<Certificado> Certificados { get; private set; } = new List<Certificado>();

        protected Aluno() { }

        public Aluno(Guid id, string nome, string email, DadosCartao dadosCartao)
        {
            Id = id;
            Nome = nome;
            Email = email;
            DadosCartao = dadosCartao;
        }

        public Aluno(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        public Aluno(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public void AdicionarMatricula(Matricula matricula)
        {
            Matriculas.Add(matricula);
        }

        public void ConcluirAula(Guid aulaId, DateTime dataConclusao)
        {
            HistoricoAprendizado.AdicionarAulaConcluida(new AulaConcluida(aulaId, dataConclusao));
        }
    }
}

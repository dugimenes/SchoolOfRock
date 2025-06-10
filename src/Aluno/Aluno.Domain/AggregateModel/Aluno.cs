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

        public void AtualizarDados(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("Nome não pode ser nulo ou vazio.", nameof(nome));
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email não pode ser nulo ou vazio.", nameof(email));
            }
            Nome = nome;
            Email = email;
        }

        public void AtualizarDadosCartao(DadosCartao dadosCartao)
        {
            DadosCartao = dadosCartao ?? throw new ArgumentNullException(nameof(dadosCartao), "Dados do cartão não podem ser nulos.");
        }

        public void ConcluirAula(Guid aulaId, DateTime dataConclusao)
        {
            HistoricoAprendizado.AdicionarAulaConcluida(new AulaConcluida(aulaId, dataConclusao));
        }
    }
}
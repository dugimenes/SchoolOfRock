namespace Pagamento.Domain.Entity
{
    public class DadosCartao
    {
        public string Numero { get; private set; }
        public string NomeTitular { get; private set; }
        public string Expiracao { get; private set; }
        public string Cvv { get; private set; }

        protected DadosCartao() { }

        public DadosCartao(string numero, string nomeTitular, string expiracao, string cvv)
        {
            Numero = numero;
            NomeTitular = nomeTitular;
            Expiracao = expiracao;
            Cvv = cvv;
        }
    }
}
namespace Conteudo.Domain.Entity
{
    public class ConteudoProgramatico
    {
        public string Descricao { get; private set; }

        protected ConteudoProgramatico() { }

        public ConteudoProgramatico(string descricao)
        {
            Descricao = descricao;
        }
    }
}
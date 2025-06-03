namespace Conteudo.Domain.AggregateModel
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
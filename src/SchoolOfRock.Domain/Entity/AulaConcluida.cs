namespace SchoolOfRock.Domain.Entity
{
    public class AulaConcluida
    {
        public Guid AulaId { get; private set; }
        public DateTime DataConclusao { get; private set; }

        protected AulaConcluida() { }

        public AulaConcluida(Guid aulaId, DateTime dataConclusao)
        {
            AulaId = aulaId;
            DataConclusao = dataConclusao;
        }
    }
}
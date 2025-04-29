namespace SchoolOfRock.Domain.ValueObjects
{
    public class HistoricoAprendizado
    {
        public List<Guid> AulasConcluidas { get; private set; } = new List<Guid>();

        protected HistoricoAprendizado() { }
    }
}
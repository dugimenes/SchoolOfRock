using SchoolOfRock.Domain.Entity;

namespace SchoolOfRock.Domain.ValueObjects
{
    public class HistoricoAprendizado
    {
        private readonly List<AulaConcluida> _aulasConcluidas = new();

        public IReadOnlyCollection<AulaConcluida> AulasConcluidas => _aulasConcluidas.AsReadOnly();

        protected HistoricoAprendizado() { }

        public void AdicionarAulaConcluida(AulaConcluida aula)
        {
            _aulasConcluidas.Add(aula);
        }
    }
}
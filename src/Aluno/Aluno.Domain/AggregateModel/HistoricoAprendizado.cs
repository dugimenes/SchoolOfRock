namespace Aluno.Domain.AggregateModel
{
    public class HistoricoAprendizado
    {
        private readonly List<AulaConcluida> _aulasConcluidas = new();

        public IReadOnlyCollection<AulaConcluida> AulasConcluidas => _aulasConcluidas.AsReadOnly();

        public HistoricoAprendizado()
        {
            
        }

        public void AdicionarAulaConcluida(AulaConcluida aula)
        {
            _aulasConcluidas.Add(aula);
        }
    }
}
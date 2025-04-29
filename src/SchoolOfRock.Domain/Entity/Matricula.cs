namespace SchoolOfRock.Domain.Entity
{
    public class Matricula : Common.Entity
    {
        public Guid CursoId { get; private set; }
        public Guid AlunoId { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public StatusMatricula Status { get; private set; }

        protected Matricula() { }

        public Matricula(Guid cursoId, Guid alunoId)
        {
            CursoId = cursoId;
            AlunoId = alunoId;
            DataMatricula = DateTime.UtcNow;
            Status = StatusMatricula.PendentePagamento;
        }

        public void ConfirmarPagamento()
        {
            Status = StatusMatricula.Ativa;
        }
    }

    public enum StatusMatricula
    {
        PendentePagamento,
        Ativa,
        Concluida
    }
}

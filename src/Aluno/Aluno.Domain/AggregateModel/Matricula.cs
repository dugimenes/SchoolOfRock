using Aluno.Domain.Enumerations;
using SchoolOfRock.Shared;

namespace Aluno.Domain.AggregateModel
{
    public class Matricula : Entity
    {
        public Guid CursoId { get; private set; }
        public Guid AlunoId { get; private set; }
        //public virtual Aluno Aluno { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public StatusMatricula Status { get; private set; }

        protected Matricula() { }

        public Matricula(Guid cursoId, Guid alunoId)
        {
            Id = Guid.NewGuid();
            CursoId = cursoId;
            AlunoId = alunoId;
            DataMatricula = DateTime.UtcNow;
            Status = StatusMatricula.PendentePagamento;
        }

        public void ConfirmarPagamento()
        {
            if (Status == StatusMatricula.Ativa)
            {
                throw new InvalidOperationException("Esta matrícula já está ativa.");
            }
            if (Status == StatusMatricula.Concluida)
            {
                throw new InvalidOperationException("Não é possível alterar uma matrícula concluída.");
            }
            Status = StatusMatricula.Ativa;
        }

        public void Concluir()
        {
            if (Status != StatusMatricula.Ativa)
            {
                throw new InvalidOperationException("A matrícula deve estar ativa para ser concluída.");
            }
            Status = StatusMatricula.Concluida;
        }
    }
}
namespace Aluno.Domain.Entity
{
    public class Certificado : Common.Entity
    {
        public Guid CursoId { get; private set; }
        public Guid AlunoId { get; private set; }
        public DateTime DataEmissao { get; private set; }

        protected Certificado() { }

        public Certificado(Guid cursoId, Guid alunoId)
        {
            CursoId = cursoId;
            AlunoId = alunoId;
            DataEmissao = DateTime.UtcNow;
        }
    }
}

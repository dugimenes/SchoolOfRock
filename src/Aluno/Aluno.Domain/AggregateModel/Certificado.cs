using SchoolOfRock.Shared;

namespace Aluno.Domain.AggregateModel
{
    public class Certificado : Entity
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

        public void AtualizarDataEmissao(DateTime novaDataEmissao)
        {
            if (novaDataEmissao <= DateTime.MinValue || novaDataEmissao > DateTime.UtcNow)
            {
                throw new ArgumentException("Data de emissão inválida.", nameof(novaDataEmissao));
            }
            DataEmissao = novaDataEmissao;
        }
    }
}

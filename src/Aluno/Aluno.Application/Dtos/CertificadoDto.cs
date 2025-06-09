namespace Aluno.Application.Dtos
{
    public class CertificadoDto
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public DateTime DataEmissao { get; set; }
    }
}

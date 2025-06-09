namespace Aluno.Application.Dtos
{
    public class HistoricoAprendizadoDto
    {
        public Guid AlunoId { get; set; }
        public List<AulaConcluidaDto> AulasConcluidas { get; set; } = new();
    }
}

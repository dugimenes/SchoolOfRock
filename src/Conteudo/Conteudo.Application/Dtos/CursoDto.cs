namespace Conteudo.Application.Dtos
{
    public class CursoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string ConteudoProgramatico { get; set; }
        public List<AulaDto> Aulas { get; set; } = new();
    }
}

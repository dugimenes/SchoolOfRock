using Aluno.Domain.Enumerations;

namespace Aluno.Application.Dtos
{
    public class MatriculaDto
    {
        public Guid Id { get; set; }
        public DateTime DataMatricula { get; set; }
        public StatusMatricula Status { get; set; }
        
    }
}
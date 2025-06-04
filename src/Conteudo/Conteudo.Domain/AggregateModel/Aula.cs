using SchoolOfRock.Shared;

namespace Conteudo.Domain.AggregateModel
{
    public class Aula : Entity
    {
        public string Titulo { get; private set; }
        public string Conteudo { get; private set; }
        public string? MaterialUrl { get; private set; }
        public Guid CursoId { get; private set; }
        public virtual Curso Curso { get; private set; }

        protected Aula() { }

        public Aula(string titulo, string conteudo, Guid cursoId, string? materialUrl = null)
        {
            Titulo = titulo;
            Conteudo = conteudo;
            CursoId = cursoId;
            MaterialUrl = materialUrl;
        }
    }
}

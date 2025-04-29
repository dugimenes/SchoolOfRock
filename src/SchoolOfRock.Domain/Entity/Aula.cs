namespace SchoolOfRock.Domain.Entity
{
    public class Aula : Common.Entity
    {
        public string Titulo { get; private set; }
        public string Conteudo { get; private set; }
        public string? MaterialUrl { get; private set; }

        public Guid CursoId { get; private set; }
        public Curso Curso { get; private set; }

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

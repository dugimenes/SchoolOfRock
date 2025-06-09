using MediatR;

namespace Conteudo.Application.Command
{
    public class AdicionarAulaCommand : IRequest<Guid>
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string MaterialUrl { get; set; }
    }
}

using MediatR;

namespace Conteudo.Application.Command
{
    public class CriarCursoCommand : IRequest<Guid> // Retorna o ID do novo curso
    {
        public string Nome { get; set; }
        public string ConteudoProgramatico { get; set; }
    }
}
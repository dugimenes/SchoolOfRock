using MediatR;

namespace SchoolOfRock.Contracts.Identity.Events
{
    public class UsuarioCriadoEvent : INotification
    {
        public Guid UsuarioId { get; }
        public string Email { get; }
        public string Nome { get; }
        public UsuarioCriadoEvent(Guid usuarioId, string email, string nome)
        {
            UsuarioId = usuarioId;
            Email = email;
            Nome = nome;
        }
    }
}
using MediatR;
using Microsoft.AspNetCore.Identity;
using SchoolOfRock.Contracts.Identity.Events;

namespace Identity.Domain.AggregateModel
{
    public class ApplicationUser : IdentityUser, IHasDomainEvents
    {
        private readonly List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public ApplicationUser()
        {
        }

        public void Cadastrar()
        {
            if (Guid.TryParse(this.Id, out var userGuid))
                AddDomainEvent(new UsuarioCriadoEvent(userGuid, Email, UserName));
            else
                throw new InvalidOperationException($"Id inválido: {Id}");
        }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }

    public interface IHasDomainEvents
    {
        public IReadOnlyCollection<INotification> DomainEvents { get; }
        public void AddDomainEvent(INotification eventItem);
        public void ClearDomainEvents();
    }
}
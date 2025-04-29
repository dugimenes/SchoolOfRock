namespace SchoolOfRock.Domain.Common
{
    public abstract class Entity
    {
        protected Entity()
        {
        }
        public Guid Id { get; set; }
    }
}
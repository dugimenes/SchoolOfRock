namespace SchoolOfRock.Shared
{
    public abstract class Entity
    {
        protected Entity()
        {
        }
        public Guid Id { get; set; }
    }
}
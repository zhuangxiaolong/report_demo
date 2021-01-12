namespace Domain.Core
{
    public class EntityBase<T, Pk> : IEntity<Pk>
        where T : IEntity
    {
        public Pk Id { get; set; }
    }
}
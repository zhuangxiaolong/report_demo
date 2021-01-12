namespace Domain.Core
{
    public interface IEntity
    {

    }

    public interface IEntity<Pk> : IEntity
    {
        Pk Id { get; set; }
    }
}
namespace Ordering.Domain.Abstractions;

public interface IAggregate<T> : IEntity<T>
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    IDomainEvent[] ClearDomainEvents();
}

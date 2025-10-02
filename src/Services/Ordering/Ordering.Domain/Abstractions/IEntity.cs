namespace Ordering.Domain.Abstractions;

public interface IEntity<T>
{
    T Id { get; }
}

using MediatR;

namespace Ordering.Domain.Abstractions
{
	internal interface IDomainEvent : INotification
	{
		Guid EventId => Guid.NewGuid();
		DateTime OccurredOn => DateTime.Now;
		public string? EventType => GetType().AssemblyQualifiedName;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstractions
{
	internal interface IAggregate<T> : IAggregate
	{
	}
	internal interface IAggregate
	{
		IReadOnlyList<IDomainEvent> DomainEvents { get; }
		IDomainEvent[] ClearDomainEvents();
	}
}

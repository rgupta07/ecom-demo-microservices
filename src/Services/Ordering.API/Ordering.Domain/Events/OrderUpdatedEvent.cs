using Ordering.Domain.Abstractions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Events
{
	internal record OrderUpdatedEvent(Order order): IDomainEvent
	{
	}
}

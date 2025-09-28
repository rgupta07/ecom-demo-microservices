using Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
	internal record OrderItemId
	{
		public Guid Value { get; }

		private OrderItemId(Guid orderItemId) => Value = orderItemId;

		public static OrderItemId Of(Guid orderItemId)
		{
			Validate(orderItemId);
			return new OrderItemId(orderItemId);
		}

		private static void Validate(Guid orderItemId)
		{
			if (orderItemId == Guid.Empty)
			{
				throw new DomainException("OrderItemId cannot be empty.");
			}
		}
	}
}

using Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
	internal record OrderId
	{
		public Guid Value { get; }

		private OrderId(Guid orderId) => Value = orderId;

		public static OrderId Of(Guid orderId)
		{
			Validate(orderId);
			return new OrderId(orderId);
		}

		private static void Validate(Guid orderId)
		{
			if (orderId == Guid.Empty)
			{
				throw new DomainException("OrderId cannot be empty.");
			}
		}
	}
}

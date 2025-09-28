using Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
	internal record OrderName
	{
		public string Value { get; } = default!;

		private OrderName(string orderName) => Value = orderName;

		public static OrderName Of(string orderName)
		{
			Validate(orderName);
			return new OrderName(orderName);
		}

		private static void Validate(string orderName)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(orderName, nameof(orderName));
			if (orderName.Length > 200)
			{
				throw new DomainException("OrderName cannot exceed 200 characters.");
			}
		}
	}
}

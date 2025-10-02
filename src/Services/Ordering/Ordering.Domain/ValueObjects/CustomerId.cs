using Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
	public record CustomerId
	{
		public Guid Value { get; }

		private CustomerId(Guid customerId) => Value = customerId;

		public static CustomerId Of(Guid customerId)
		{
			Validate(customerId);
			return new CustomerId(customerId);
		}

		private static void Validate(Guid customerId)
		{
			if (customerId == Guid.Empty)
			{
				throw new DomainException("CustomerId cannot be empty.");
			}
		}

	}
}

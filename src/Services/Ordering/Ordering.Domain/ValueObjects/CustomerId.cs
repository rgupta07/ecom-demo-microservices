using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects
{
	public record CustomerId
	{
		private CustomerId() { }

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

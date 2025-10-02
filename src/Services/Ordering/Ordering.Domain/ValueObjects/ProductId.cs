using Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
	public class ProductId
	{
		public Guid Value { get; }

		private ProductId(Guid productId) => Value = productId;

		public static ProductId Of(Guid productId)
		{
			Validate(productId);
			return new ProductId(productId);
		}

		private static void Validate(Guid productId)
		{
			if (productId == Guid.Empty)
			{
				throw new DomainException("ProductId cannot be empty.");
			}
		}
	}
}

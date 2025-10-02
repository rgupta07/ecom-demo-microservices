using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
	public class Product: Entity<ProductId>
	{
		public string Name { get; private set; } = default!;
		public decimal Price { get; private set; }
		private Product(ProductId id, string name, decimal price)
		{
			Id = id;
			Name = name;
			Price = price;
		}
		public static Product Create(ProductId id, string name, decimal price)
		{
			Validate(id, name, price);
			return new Product(id, name, price);
		}
		private static void Validate(ProductId productId, string name, decimal price)
		{
			ArgumentNullException.ThrowIfNull(productId);
			ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
			if (price < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
			}
		}

	}
}

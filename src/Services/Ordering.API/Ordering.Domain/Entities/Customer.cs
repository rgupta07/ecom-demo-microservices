using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
	internal class Customer: Entity<CustomerId>
	{
		public string Name { get; private set; } = default!;
		public string Email { get; private set; } = default!;

		private Customer(CustomerId id, string name, string email)	
		{
			Id = id;
			Name = name;
			Email = email;
		}

		public static Customer Create(CustomerId id, string name, string email)
		{
			Validate(id, name, email);
			return new Customer(id, name, email);
		}

		private static void Validate(CustomerId customerId, string name, string email)
		{
			ArgumentNullException.ThrowIfNull(customerId);
			ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
			ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
		}
	}
}

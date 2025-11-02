using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
	public record Payment
	{
		public string? CardName { get; } = default!;
		public string CardNumber { get; } = default!;
		public string Expiration { get; } = default!;
		public string Cvv { get; } = default!;
		public PaymentMethod PaymentMethod { get; } = default!;

		private Payment() { }

		private Payment(string cardName, string cardNumber, string expiration, string cvv, PaymentMethod paymentMethod)
		{
			CardName = cardName;
			CardNumber = cardNumber;
			Expiration = expiration;
			Cvv = cvv;
			PaymentMethod = paymentMethod;
		}

		public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, PaymentMethod paymentMethod)
		{
			Validate(cardName, cardNumber, cvv);

			return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
		}

		private static void Validate(string cardName, string cardNumber, string cvv)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
			ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
			ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
		}
	}
}

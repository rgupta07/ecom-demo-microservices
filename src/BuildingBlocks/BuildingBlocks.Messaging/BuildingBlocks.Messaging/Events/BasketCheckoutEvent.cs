using System.Text;

namespace BuildingBlocks.Messaging.Events;

public record BasketCheckoutEvent : IntegrationEvent
{
	public string UserName { get; set; } = default!;
	public Guid CustomerId { get; set; } = default!;
	public decimal TotalPrice { get; set; } = default!;

	// ShippingAddress
	public string ShippingAddressFirstName { get; set; } = default!;
	public string ShippingAddressLastName { get; set; } = default!;
	public string ShippingAddressEmailAddress { get; set; } = default!;
	public string ShippingAddressLine { get; set; } = default!;
	public string ShippingAddressCountry { get; set; } = default!;
	public string ShippingAddressState { get; set; } = default!;
	public string ShippingAddressZipCode { get; set; } = default!;

	//Billing Address
	public string BillingAddressFirstName { get; set; } = default!;
	public string BillingAddressLastName { get; set; } = default!;
	public string BillingAddressEmailAddress { get; set; } = default!;
	public string BillingAddressLine { get; set; } = default!;
	public string BillingAddressCountry { get; set; } = default!;
	public string BillingAddressState { get; set; } = default!;
	public string BillingAddressZipCode { get; set; } = default!;

	//OrderItems
	public List<OrderItem> OrderItems { get; set; } = [];

	// Payment
	public string CardName { get; set; } = default!;
	public string CardNumber { get; set; } = default!;
	public string Expiration { get; set; } = default!;
	public string CVV { get; set; } = default!;
	public int PaymentMethod { get; set; } = default!;
}

public record OrderItem(
	Guid OrderId,
	Guid ProductId,
	int Quantity,
	decimal Price
);
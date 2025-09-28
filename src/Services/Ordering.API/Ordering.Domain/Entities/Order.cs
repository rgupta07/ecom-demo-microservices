using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.Events;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Entities
{
	internal class Order : Aggregate<OrderId>
	{
		private readonly List<OrderItem> _orderItems = [];
		public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

		public CustomerId CustomerId { get; private set; } = default!;
		public OrderName OrderName { get; private set; } = default!;
		public Address ShippingAddress { get; private set; } = default!;
		public Address BillingAddress { get; private set; } = default!;
		public Payment Payment { get; private set; } = default!;
		public OrderStatus Status { get; private set; } = OrderStatus.Pending;
		public decimal TotalPrice
		{
			get => OrderItems.Sum(x => x.Price * x.Quantity);
			private set { }
		}


		private Order(CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, List<OrderItem> orderItems)
		{
			Id = OrderId.Of(Guid.NewGuid());
			CustomerId = customerId;
			OrderName = orderName;
			ShippingAddress = shippingAddress;
			BillingAddress = billingAddress;
			Payment = payment;
		}

		public static Order Create(CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, List<OrderItem> orderItems)
		{
			Validate(customerId, orderName, shippingAddress, billingAddress, payment, orderItems);
			
			var order = new Order(customerId, orderName, shippingAddress, billingAddress, payment, orderItems);

			order._orderItems.AddRange(orderItems);

			order.AddDomainEvent(new OrderCreatedEvent(order));

			return order;
		}

		public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
		{
			OrderName = orderName;
			ShippingAddress = shippingAddress;
			BillingAddress = billingAddress;
			Payment = payment;
			Status = status;

			AddDomainEvent(new OrderUpdatedEvent(this));
		}

		public void Add(ProductId productId, int quantity, decimal price)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

			var orderItem = OrderItem.Create(this.Id, productId, quantity, price);

			_orderItems.Add(orderItem);
		}

		public void Remove(ProductId productId)
		{
			var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
			if (orderItem is not null)
			{
				_orderItems.Remove(orderItem);
			}
		}

		private static void Validate(CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, List<OrderItem> orderItems)
		{
			ArgumentNullException.ThrowIfNull(customerId);
			ArgumentNullException.ThrowIfNull(orderName);
			ArgumentNullException.ThrowIfNull(shippingAddress);
			ArgumentNullException.ThrowIfNull(billingAddress);
			ArgumentNullException.ThrowIfNull(payment);
			if (orderItems == null || !orderItems.Any())
			{
				throw new ArgumentException("Order must have at least one order item.", nameof(orderItems));
			}
		}
	}
}

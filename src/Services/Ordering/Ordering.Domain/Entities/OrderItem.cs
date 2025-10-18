using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities;

public class OrderItem: Entity<OrderItemId>
{
    private OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        Id = OrderItemId.Of(Guid.NewGuid());
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
    public OrderId OrderId { get; private set; } = default!;
    public ProductId ProductId { get; private set; } = default!;
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    private OrderItem() { }

	public static OrderItem Create(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        Validate(orderId, productId, quantity, price);

        return new OrderItem(orderId, productId, quantity, price);
    }

    public static void Validate(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        ArgumentNullException.ThrowIfNull(orderId, nameof(orderId));
        ArgumentNullException.ThrowIfNull(productId, nameof(productId));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(price, 10_000);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);
    }
}

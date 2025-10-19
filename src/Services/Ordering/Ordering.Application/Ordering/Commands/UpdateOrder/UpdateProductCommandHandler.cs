using Microsoft.EntityFrameworkCore;
using Ordering.Application.Exceptions;
using Ordering.Application.Ordering.Commands.UpdateOrder;

namespace Ordering.Application.Ordering.Commands.CreateOrder;

public class UpdateOrderCommandHandler(IApplicationDbContext _dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
	public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _dbContext.Orders.FindAsync(OrderId.Of(request.Order.Id), cancellationToken) ?? throw new OrderNotFoundException(request.Order.Id);

		var updatedOrder = UpdateOrder(request.Order, order);

		_dbContext.Orders.Update(updatedOrder);

		await _dbContext.SaveChangesAsync(cancellationToken);

		return new UpdateOrderResult(updatedOrder.Id.Value);
	}
	
	private static Order UpdateOrder(OrderDto request, Order order)
	{
		var orderName = OrderName.Of(request.OrderName);
		var shippingAddress = Address.Of(request.ShippingAddress.FirstName, request.ShippingAddress.LastName, request.ShippingAddress.EmailAddress, request.ShippingAddress.AddressLine, request.ShippingAddress.Country, request.ShippingAddress.State, request.ShippingAddress.ZipCode);
		var billingAddress = Address.Of(request.BillingAddress.FirstName, request.BillingAddress.LastName, request.BillingAddress.EmailAddress, request.BillingAddress.AddressLine, request.BillingAddress.Country, request.BillingAddress.State, request.BillingAddress.ZipCode);
		var payment = Payment.Of(request.Payment.CardName, request.Payment.CardNumber, request.Payment.Expiration, request.Payment.Cvv, request.Payment.PaymentMethod);

		order.Update(orderName, shippingAddress, billingAddress, payment, request.Status);

		return order;
	}
}

public class UpdateOrderCommandHandlerValidator: AbstractValidator<UpdateOrderCommand>
{
	public UpdateOrderCommandHandlerValidator()
	{
		RuleFor(x => x.Order.Id)
			.NotEmpty()
			.WithMessage("OrderId is required");

		RuleFor(x => x.Order.OrderName)
			.NotEmpty()
			.WithMessage("OrderName is required")
			.MaximumLength(100)
			.WithMessage("OrderName must not exceed 100 characters");

		RuleFor(x => x.Order.OrderItems)
			.NotEmpty()
			.WithMessage("Order must contain at least one item")
			.Must(items => items != null && items.Any())
			.WithMessage("Order items collection cannot be empty");

		RuleFor(x => x.Order.ShippingAddress)
			.NotEmpty()
			.WithMessage("ShippingAddress is required");

		RuleFor(x => x.Order.BillingAddress)
			.NotEmpty()
			.WithMessage("BillingAddress is required");

		RuleFor(x => x.Order.Payment)
			.NotEmpty()
			.WithMessage("Payment is required");

		RuleForEach(x => x.Order.OrderItems).ChildRules(orderItem =>
		{
			orderItem.RuleFor(x => x.ProductId)
				.NotEmpty()
				.WithMessage("OrderItem ProductId is required");
			orderItem.RuleFor(x => x.Quantity)
				.GreaterThan(0)
				.WithMessage("OrderItem Quantity must be greater than 0");
			orderItem.RuleFor(x => x.Price)
				.GreaterThan(0)
				.WithMessage("OrderItem Price must be greater than 0");
		});
	}
}
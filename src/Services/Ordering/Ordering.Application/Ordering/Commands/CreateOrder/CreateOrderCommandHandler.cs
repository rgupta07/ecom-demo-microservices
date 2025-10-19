using FluentValidation;
using Mapster;
using Ordering.Application.DTOs;
using Ordering.Domain.Entities;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Ordering.Commands.CreateOrder;

public class CreateOrderCommandHandler(IApplicationDbContext _dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
	public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
	{
		var order = CreateOrder(request.Order);

		_dbContext.Orders.Add(order);

		await _dbContext.SaveChangesAsync(cancellationToken);

		return new CreateOrderResult(order.Id.Value);
	}

	private static Order CreateOrder(OrderDto request)
	{
		var customerId = CustomerId.Of(request.CustomerId);
		var orderName = OrderName.Of(request.OrderName);
		var shippingAddress = Address.Of(request.ShippingAddress.FirstName, request.ShippingAddress.LastName, request.ShippingAddress.EmailAddress, request.ShippingAddress.AddressLine, request.ShippingAddress.Country, request.ShippingAddress.State, request.ShippingAddress.ZipCode);
		var billingAddress = Address.Of(request.BillingAddress.FirstName, request.BillingAddress.LastName, request.BillingAddress.EmailAddress, request.BillingAddress.AddressLine, request.BillingAddress.Country, request.BillingAddress.State, request.BillingAddress.ZipCode);
		var payment = Payment.Of(request.Payment.CardName, request.Payment.CardNumber, request.Payment.Expiration, request.Payment.Cvv, request.Payment.PaymentMethod);
		var orderItems = request.OrderItems
			.Select(oi =>
				OrderItem.Create(
					OrderId.Of(oi.OrderId),
					ProductId.Of(oi.ProductId),
					oi.Quantity,
					oi.Price)
			).ToList();

		var order = Order.Create(customerId, orderName, shippingAddress, billingAddress, payment, orderItems);

		return order;
	}
}

public class CreateOrderCommandHandlerValidator : AbstractValidator<CreateOrderCommand>
{
	public CreateOrderCommandHandlerValidator()
	{
		RuleFor(x => x.Order.CustomerId)
			.NotNull().WithMessage("CustomerId cannot be null!");
		
		RuleFor(x => x.Order.OrderName)
			.NotNull().WithMessage("OrderName cannot be null!");
		
		RuleFor(x => x.Order.OrderItems)
			.NotEmpty().WithMessage("OrderItems cannot be empty!");
		
		RuleFor(x => x.Order.ShippingAddress)
			.NotNull().WithMessage("ShippingAddress cannot be null!");
		
		RuleFor(x => x.Order.BillingAddress)
			.NotNull().WithMessage("BillingAddress cannot be null!");
		
		RuleFor(x => x.Order.Payment)
			.NotNull().WithMessage("Payment cannot be null!");
		
		RuleForEach(x => x.Order.OrderItems).ChildRules(orderItem =>
		{
			orderItem.RuleFor(x => x.ProductId)
				.NotNull().WithMessage("OrderItem ProductId cannot be null!");
			
			orderItem.RuleFor(x => x.Quantity)
				.GreaterThan(0).WithMessage("OrderItem Quantity must be greater than 0!");
			
			orderItem.RuleFor(x => x.Price)
				.GreaterThan(0).WithMessage("OrderItem Price must be greater than 0!");
		});
	}
}
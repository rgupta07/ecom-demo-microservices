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
		var shippingAddressDto = request.ShippingAddress;
		var billingAddressDto = request.BillingAddress;
		var paymentDto = request.Payment;

		var customerId = CustomerId.Of(request.CustomerId);

		var orderName = OrderName.Of(request.OrderName);

		var shippingAddress = Address.Of(
			shippingAddressDto.FirstName, 
			shippingAddressDto.LastName, 
			shippingAddressDto.EmailAddress,
			shippingAddressDto.AddressLine,
			shippingAddressDto.Country, 
			shippingAddressDto.State,
			shippingAddressDto.ZipCode);

		var billingAddress = Address.Of(
			billingAddressDto.FirstName, 
			billingAddressDto.LastName, 
			billingAddressDto.EmailAddress, 
			billingAddressDto.AddressLine, 
			billingAddressDto.Country, 
			billingAddressDto.State,
			billingAddressDto.ZipCode);

		var payment = Payment.Of(
			paymentDto.CardName, 
			paymentDto.CardNumber, 
			paymentDto.Expiration, 
			paymentDto.Cvv,
			paymentDto.PaymentMethod);

		var order = Order.Create(customerId, orderName, shippingAddress, billingAddress, payment);

		foreach(var orderItemDto in request.OrderItems)
		{
			var productId = ProductId.Of(orderItemDto.ProductId);
			order.Add(productId, orderItemDto.Quantity, orderItemDto.Price);
		}

		return order;
	}
}

public class CreateOrderCommandHandlerValidator : AbstractValidator<CreateOrderCommand>
{
	public CreateOrderCommandHandlerValidator()
	{
		RuleFor(x => x)
			.NotNull().WithMessage("Order cannot be null");

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
using Basket.API.DTOs;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.ShoppingCartFeature.CheckoutShoppingCartFeature;

public record CheckoutShoppingCartCommand(ShoppingCartCheckoutDto CartCheckoutDto) : ICommand<CheckoutShoppingCartResult>;
public record CheckoutShoppingCartResult(bool IsSuccess);

public class CheckoutShoppingCartHandler(IShopingCartRepository shopingCartRepository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutShoppingCartCommand, CheckoutShoppingCartResult>
{
	public async Task<CheckoutShoppingCartResult> Handle(CheckoutShoppingCartCommand command, CancellationToken cancellationToken)
	{
		var cart = await shopingCartRepository.GetShoppingCart(command.CartCheckoutDto.UserName);

		if(cart == null)
			return new CheckoutShoppingCartResult(false);

		var eventMessage = command.CartCheckoutDto.Adapt<BasketCheckoutEvent>();
		eventMessage.TotalPrice = cart.ShoppingCart.TotalPrice;

		await publishEndpoint.Publish(eventMessage, cancellationToken);

		await shopingCartRepository.DeleteShoppingCart(command.CartCheckoutDto.UserName);

		return new CheckoutShoppingCartResult(true);
	}
}

public class CheckoutShoppingCartCommandValidator : AbstractValidator<CheckoutShoppingCartCommand>
{
	public CheckoutShoppingCartCommandValidator()
	{
		RuleFor(x => x.CartCheckoutDto)
			.NotNull().NotEmpty().WithMessage("Shopping cart cannot be empty");
		RuleFor(x => x.CartCheckoutDto.UserName)
			.NotNull().NotEmpty().WithMessage("User name is required");
		RuleFor(x => x.CartCheckoutDto.CustomerId)
			.NotNull().NotEmpty().WithMessage("Customer ID is required");
		RuleFor(x => x.CartCheckoutDto.TotalPrice)
			.NotNull().NotEmpty().WithMessage("Total price is required")
			.GreaterThan(0).WithMessage("Total price must be greater than 0");

		// Shipping Address Validations
		RuleFor(x => x.CartCheckoutDto.ShippingAddressFirstName)
			.NotNull().NotEmpty().WithMessage("Shipping address first name is required");
		RuleFor(x => x.CartCheckoutDto.ShippingAddressLastName)
			.NotNull().NotEmpty().WithMessage("Shipping address last name is required");
		RuleFor(x => x.CartCheckoutDto.ShippingAddressEmailAddress)
			.NotNull().NotEmpty().WithMessage("Shipping address email is required")
			.EmailAddress().WithMessage("Invalid email address format");
		RuleFor(x => x.CartCheckoutDto.ShippingAddressLine)
			.NotNull().NotEmpty().WithMessage("Shipping address line is required");
		RuleFor(x => x.CartCheckoutDto.ShippingAddressCountry)
			.NotNull().NotEmpty().WithMessage("Shipping address country is required");
		RuleFor(x => x.CartCheckoutDto.ShippingAddressState)
			.NotNull().NotEmpty().WithMessage("Shipping address state is required");
		RuleFor(x => x.CartCheckoutDto.ShippingAddressZipCode)
			.NotNull().NotEmpty().WithMessage("Shipping address zip code is required");

		// Billing Address Validations
		RuleFor(x => x.CartCheckoutDto.BillingAddressFirstName)
			.NotNull().NotEmpty().WithMessage("Billing address first name is required");
		RuleFor(x => x.CartCheckoutDto.BillingAddressLastName)
			.NotNull().NotEmpty().WithMessage("Billing address last name is required");
		RuleFor(x => x.CartCheckoutDto.BillingAddressEmailAddress)
			.NotNull().NotEmpty().WithMessage("Billing address email is required")
			.EmailAddress().WithMessage("Invalid email address format");
		RuleFor(x => x.CartCheckoutDto.BillingAddressLine)
			.NotNull().NotEmpty().WithMessage("Billing address line is required");
		RuleFor(x => x.CartCheckoutDto.BillingAddressCountry)
			.NotNull().NotEmpty().WithMessage("Billing address country is required");
		RuleFor(x => x.CartCheckoutDto.BillingAddressState)
			.NotNull().NotEmpty().WithMessage("Billing address state is required");
		RuleFor(x => x.CartCheckoutDto.BillingAddressZipCode)
			.NotNull().NotEmpty().WithMessage("Billing address zip code is required");

		// Payment Validations
		RuleFor(x => x.CartCheckoutDto.CardName)
			.NotNull().NotEmpty().WithMessage("Card name is required");
		RuleFor(x => x.CartCheckoutDto.CardNumber)
			.NotNull().NotEmpty().WithMessage("Card number is required")
			.CreditCard().WithMessage("Invalid card number format");
		RuleFor(x => x.CartCheckoutDto.Expiration)
			.NotNull().NotEmpty().WithMessage("Card expiration date is required");
		RuleFor(x => x.CartCheckoutDto.CVV)
			.NotNull().NotEmpty().WithMessage("CVV is required")
			.Length(3, 4).WithMessage("CVV must be 3 or 4 digits");
		RuleFor(x => x.CartCheckoutDto.PaymentMethod)
			.NotNull().NotEmpty().WithMessage("Payment method is required")
			.InclusiveBetween(1, 3).WithMessage("Invalid payment method");
	}
}

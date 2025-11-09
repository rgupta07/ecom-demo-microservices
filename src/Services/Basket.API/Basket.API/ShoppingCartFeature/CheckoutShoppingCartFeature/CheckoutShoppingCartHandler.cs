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

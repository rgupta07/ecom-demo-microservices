using Basket.API.Exceptions;
using Basket.API.Models;
using Basket.API.Repository;
using BuildingBlocks.CQRS;
using System.Windows.Input;

namespace Basket.API.ShoppingCartFeature.UpdateShoppingCartFeature
{
	public record UpdateShoppingCartCommand(ShoppingCart cart) : ICommand<UpdateShoppingCartResult>;
	public record UpdateShoppingCartResult(bool IsSuccess);
	public class UpdateShoppingCartHandler(IShopingCartRepository repository): ICommandHandler<UpdateShoppingCartCommand, UpdateShoppingCartResult>
	{
		public async Task<UpdateShoppingCartResult> Handle(UpdateShoppingCartCommand command, CancellationToken cancellationToken)
		{
			var result = await repository.UpdateShoppingCart(command.cart);

			if(result.IsSuccess)
			{
				return result;
			}

			throw new CartNotFoundException(command.cart.UserName);
		}
	}
}

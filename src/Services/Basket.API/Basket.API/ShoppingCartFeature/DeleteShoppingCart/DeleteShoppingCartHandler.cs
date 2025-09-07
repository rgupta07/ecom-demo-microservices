using Basket.API.Repository;
using BuildingBlocks.CQRS;

namespace Basket.API.ShoppingCartFeature.DeleteShoppingCart
{
	public record DeleteShoppingCartCommand(string UserName) : ICommand<DeleteShoppingCartResult>;
	public record DeleteShoppingCartResult(bool Success);
	public class DeleteShoppingCartHandler(IShopingCartRepository repository) : ICommandHandler<DeleteShoppingCartCommand, DeleteShoppingCartResult>
	{
		public async Task<DeleteShoppingCartResult> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
		{
			return await repository.DeleteShoppingCart(request.UserName);
		}
	}
}

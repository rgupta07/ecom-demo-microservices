
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

	public class UpdateShoppingCartHandlerValidator : AbstractValidator<UpdateShoppingCartCommand>
	{
		public UpdateShoppingCartHandlerValidator()
		{
			RuleFor(x => x.cart.UserName).NotEmpty().WithMessage("UserName is required.");
			RuleFor(x => x.cart.Items).NotNull().WithMessage("Cart items cannot be null.");
		}
	}
}

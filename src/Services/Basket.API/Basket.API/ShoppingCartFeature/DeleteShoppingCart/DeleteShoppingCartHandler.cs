
namespace Basket.API.ShoppingCartFeature.DeleteShoppingCart
{
	public record DeleteShoppingCartCommand(string UserName) : ICommand<DeleteShoppingCartResult>;
	public record DeleteShoppingCartResult(bool IsSuccess);
	public class DeleteShoppingCartHandler(IShopingCartRepository repository) : ICommandHandler<DeleteShoppingCartCommand, DeleteShoppingCartResult>
	{
		public async Task<DeleteShoppingCartResult> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
		{
			return await repository.DeleteShoppingCart(request.UserName);
		}
	}

	public class DeleteShoppingCartHandlerValidator : AbstractValidator<DeleteShoppingCartCommand>
	{
		public DeleteShoppingCartHandlerValidator()
		{
			RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.");
		}
	}
}

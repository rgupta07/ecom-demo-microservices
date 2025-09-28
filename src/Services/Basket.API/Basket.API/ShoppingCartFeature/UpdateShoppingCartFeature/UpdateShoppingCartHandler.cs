
using Discount.gRPC.Protos;

namespace Basket.API.ShoppingCartFeature.UpdateShoppingCartFeature
{
	public record UpdateShoppingCartCommand(ShoppingCart cart) : ICommand<UpdateShoppingCartResult>;
	public record UpdateShoppingCartResult(bool IsSuccess);
	public class UpdateShoppingCartHandler(IShopingCartRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProtoClient) : ICommandHandler<UpdateShoppingCartCommand, UpdateShoppingCartResult>
	{
		public async Task<UpdateShoppingCartResult> Handle(UpdateShoppingCartCommand command, CancellationToken cancellationToken)
		{
			await ApplyDiscount(command.cart);

			var result = await repository.UpdateShoppingCart(command.cart);

			if(result.IsSuccess)
			{
				return result;
			}

			throw new CartNotFoundException(command.cart.UserName);
		}
		public async Task ApplyDiscount(ShoppingCart cart)
		{
			foreach (var item in cart.Items)
			{
				var request = new GetDiscountRequest
				{
					ProductName = item.ProductName
				};

				var coupon = await discountProtoClient.GetDiscountAsync(request, cancellationToken: CancellationToken.None);

				if (coupon != null)
				{
					item.Price -= (decimal)coupon.Discount.DiscountAmount;
				}

			}
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

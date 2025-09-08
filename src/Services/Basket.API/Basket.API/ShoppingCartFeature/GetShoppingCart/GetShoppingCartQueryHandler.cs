
namespace Basket.API.ShoppingCartFeature.GetShoppingCart
{
	public record GetShoppingCartQuery(string UserName): IQuery<GetShoppingCartResult>;
	public record GetShoppingCartResult(ShoppingCart ShoppingCart);

	public class GetShoppingCartQueryHandler(IShopingCartRepository repository) : IQueryHandler<GetShoppingCartQuery, GetShoppingCartResult>
	{
		public async Task<GetShoppingCartResult> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
		{
			return await repository.GetShoppingCart(request.UserName);
		}
	}

	public class GetShoppingCartQueryHandlerValidator : AbstractValidator<GetShoppingCartQuery>
	{
		public GetShoppingCartQueryHandlerValidator()
		{
			RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.");
		}
	}
}

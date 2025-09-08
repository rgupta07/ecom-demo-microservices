
namespace Basket.API.ShoppingCartFeature.UpdateShoppingCartFeature
{
	public class UpdateShoppingCartEndpoint() : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPost("/basket/{userName}", async (ISender sender, [FromRoute] string userName, [FromBody] UpdateShoppingCartRequest request) =>
			{
				var command = new UpdateShoppingCartCommand(new ShoppingCart(userName)
				{
					Items = [..request.CartItems]
				});

				var result = await sender.Send(command);

				return result.Adapt<UpdateShoppingCartResponse>();
			})
			.WithName("UpdateShoppingCart")
			.WithSummary("Update a shopping cart")
			.Produces(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.ProducesProblem(StatusCodes.Status500InternalServerError);
		}
	}

	public record UpdateShoppingCartRequest(IList<ShoppingCartItem> CartItems);
	public record UpdateShoppingCartResponse(bool IsSuccess);
}

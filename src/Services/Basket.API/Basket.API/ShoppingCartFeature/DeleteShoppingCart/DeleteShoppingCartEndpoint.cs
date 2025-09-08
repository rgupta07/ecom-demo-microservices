
namespace Basket.API.ShoppingCartFeature.DeleteShoppingCart
{
	public class DeleteShoppingCartEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapDelete("basket/{userName}", async(ISender sender, string userName) =>
			{
				var command = new DeleteShoppingCartCommand(userName);

				var result = await sender.Send(command);

				var response = result.Adapt<DeleteShoppingCartResponse>();

				return response;

			})
			.WithName("DeleteShoppingCart")
			.WithSummary("Delete a shopping cart by user name")
			.Produces(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status404NotFound);
		}
	}

	public record DeleteShoppingCartResponse(bool IsSuccess);
}

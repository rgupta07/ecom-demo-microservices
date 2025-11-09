using Basket.API.DTOs;

namespace Basket.API.ShoppingCartFeature.CheckoutShoppingCartFeature
{
	public record CheckoutShoppingCartRequest(ShoppingCartCheckoutDto CartCheckoutDto);
	public record CheckoutShoppingCartResponse(bool IsSuccess);
	public class CheckoutShoppingCartEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPost("basket/checkout", async (CheckoutShoppingCartRequest request, IMediator mediator) =>
			{
				var command = request.Adapt<CheckoutShoppingCartCommand>();

				var result = await mediator.Send(command);

				var response = result.Adapt<CheckoutShoppingCartResponse>();

				return Results.Ok(response);
			})
			.WithName("CheckoutShoppingCart")
			.WithSummary("Checkout Shopping Cart")
			.WithDescription("Checkout the shopping cart and create an order.")
			.Accepts<CheckoutShoppingCartRequest>("application/json")
			.Produces<CheckoutShoppingCartResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest);
		}
	}
}

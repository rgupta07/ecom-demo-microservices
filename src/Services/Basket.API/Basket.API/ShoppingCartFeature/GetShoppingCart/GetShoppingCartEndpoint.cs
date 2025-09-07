using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.ShoppingCartFeature.GetShoppingCart
{
	public class GetShoppingCartEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/basket/{userName}", async (ISender sender, string userName) =>
			{
				var query = new GetShoppingCartQuery(userName);

				var result = await sender.Send(query);

				return result.Adapt<GetShoppingCartResponse>();
			})
			.WithName("GetShoppingCart")
			.WithSummary("Get a shopping cart by user name")
			.Produces<ShoppingCart>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status404NotFound);
		}
	}

	public record GetShoppingCartResponse(ShoppingCart ShoppingCart);
}

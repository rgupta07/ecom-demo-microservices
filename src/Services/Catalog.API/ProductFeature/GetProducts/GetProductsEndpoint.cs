
using Catalog.API.Models;
using Microsoft.AspNetCore.Components;

namespace Catalog.API.ProductFeature.GetProducts
{
	public class GetProductsEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
			{
				var query = request.Adapt<GetProductsQuery>();

				var result =  await sender.Send(query);

				var response = result.Adapt<GetProductsResult>();

				return response;
			})
			.WithName("GetProducts")
			.WithSummary("Get all products")
			.Produces<GetProductsResult>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.ProducesProblem(StatusCodes.Status400BadRequest);
		}

		public record GetProductsRequest(int? PageNumber, int? PageSize);
		public record GetProductsResult(IEnumerable<Product> Products, long TotalCount);
	}
}

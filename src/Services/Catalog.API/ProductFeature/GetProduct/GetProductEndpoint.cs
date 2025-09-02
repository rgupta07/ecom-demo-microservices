

namespace Catalog.API.ProductFeature.GetProduct
{
	public class GetProductEndpoint: ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/product/{id:guid}", async (ISender sender, Guid id) =>
			{
				var request = new GetProductRequest(id);

				var query = request.Adapt<GetProductQuery>();

				var result =  await sender.Send(query);

				var response = result.Adapt<GetProductResponse>();

				return response;
			})
			.WithName("GetProduct")
			.WithSummary("Get a product by ID")
			.Produces<GetProductResult>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.ProducesProblem(StatusCodes.Status400BadRequest);
		}
		public record GetProductRequest(Guid Id);
		public record GetProductResponse(string Name, string Description, IList<string> Categories, string ImgFile, decimal Price);
	}
}

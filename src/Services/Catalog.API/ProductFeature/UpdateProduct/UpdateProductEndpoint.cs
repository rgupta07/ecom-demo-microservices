
namespace Catalog.API.ProductFeature.UpdateProduct
{
	public class UpdateProductEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPut("/product", async (ISender sender, UpdateProductRequest request) =>
			{
				var command = request.Adapt<UpdateProductCommand>();
				await sender.Send(command);
				return Results.NoContent();
			})
			.WithName("UpdateProduct")
			.WithSummary("Update an existing product")
			.Produces(StatusCodes.Status204NoContent)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.ProducesProblem(StatusCodes.Status400BadRequest);
		}
	}

	public record UpdateProductRequest(
		Guid Id,
		string Name,
		string Description,
		IList<string> Category,
		string ImgFile,
		decimal Price);
}

namespace Catalog.API.ProductFeature.DeleteProduct
{
	public class DeleteProductEndpoint: ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapDelete("/product/{id:guid}", async (ISender sender, Guid id) =>
			{
				var request = new DeleteProductCommand(id);
				var command = request.Adapt<DeleteProductCommand>();

				return await sender.Send(command);
			})
			.WithName("DeleteProduct")
			.WithSummary("Delete a product by ID")
			.Produces(StatusCodes.Status204NoContent)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.ProducesProblem(StatusCodes.Status400BadRequest);
		}
	}
}

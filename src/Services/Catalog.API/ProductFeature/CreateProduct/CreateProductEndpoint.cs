namespace Catalog.API.ProductFeature.CreateProduct
{
	public record CreateProductRequest(
		string Name,
		string Description,
		IList<string> Categories,
		string ImgFile,
		decimal Price);
	public class CreateProductEndpoint : ICarterModule
	{
        public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPost("/product", async (ISender sender, CreateProductRequest request) =>
			{
				var command = request.Adapt<CreateProductCommand>();
				return await sender.Send(command);
			})
			.WithName("CreateProduct")
			.WithSummary("Create a new product")
			.Produces<CreateProductResult>(StatusCodes.Status201Created)
			.ProducesProblem(StatusCodes.Status400BadRequest);
		}
	}
}

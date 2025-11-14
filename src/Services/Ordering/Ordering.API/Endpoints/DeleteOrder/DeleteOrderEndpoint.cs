using Ordering.Application.Ordering.Commands.DeleteOrder;

namespace Ordering.API.Endpoints.DeleteOrder;

public record DeleteEndpointResponse(bool IsSuccess);

public class DeleteOrderEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/orders/{id}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
		{
			var command = new DeleteOrderCommand(id);

			var result = await mediator.Send(command, cancellationToken);

			var response = result.Adapt<DeleteEndpointResponse>();

			return response.Adapt<DeleteEndpointResponse>();
		})
		.WithName("DeleteOrder")
		.WithSummary("Delete Order")
		.WithDescription("Deletes an order by its unique identifier")
		.Produces<DeleteEndpointResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound);
	}
}

using Ordering.Application.Ordering.Commands.UpdateOrder;

namespace Ordering.API.Endpoints.UpdateOrder;

public record UpdateOrderRequest(OrderDto Order);
public record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrderEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPut("/orders", async (UpdateOrderRequest request, IMediator mediator, CancellationToken cancellationToken) =>
		{
			var command = new UpdateOrderCommand(request.Order);

			var result =  await mediator.Send(command, cancellationToken);

			var response = result.Adapt<UpdateOrderResponse>();

			return response;
		})
		.WithName("UpdateOrder")
		.Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
		.ProducesValidationProblem()
		.Produces(StatusCodes.Status404NotFound);
	}	
}

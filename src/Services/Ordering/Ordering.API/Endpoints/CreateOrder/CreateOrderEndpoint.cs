using Ordering.Application.Ordering.Commands.CreateOrder;

namespace Ordering.API.Endpoints.CreateOrder;

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid OrderId);

public class CreateOrderEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/orders", async (CreateOrderRequest request, IMediator mediator, CancellationToken cancellationToken) =>
		{
			var command = new CreateOrderCommand(request.Order);

			var result = await mediator.Send(command, cancellationToken);

			var response = result.Adapt<CreateOrderResponse>();

			return response;
		})
		.WithName("CreateOrder")
		.WithDescription("Creates a new order")
		.WithSummary("Create Order")
		.Produces<CreateOrderResponse>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesValidationProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status500InternalServerError)
		.WithOpenApi();
	}
}

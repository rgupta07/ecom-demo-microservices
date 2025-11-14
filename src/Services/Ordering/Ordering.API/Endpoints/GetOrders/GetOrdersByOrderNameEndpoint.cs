using BuildingBlocks.Pagination;
using Ordering.Application.Ordering.Queries.GetOrdersByOrderName;

namespace Ordering.API.Endpoints.GetOrders;

public record GetOrdersByOrderNameRequest: PaginationRequest
{
	public string OrderName { get; init; } = string.Empty;
}
public record GetOrdersByOrderNameResponse(PaginatedResult<OrderDto> Orders);

public class GetOrdersByOrderNameEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/orders/by-name/{orderName}", async ([AsParameters] GetOrdersByOrderNameRequest request, IMediator mediator, CancellationToken cancellationToken) =>
		{
			var query = new GetOrdersByOrderNameQuery(request.OrderName, request.PageNumber, request.PageSize);

			var result = await mediator.Send(query, cancellationToken);

			var response = result.Adapt<GetOrdersByOrderNameResult>();

			return response;
		})
		.WithName("GetOrdersByOrderName")
		.WithSummary("Get Orders By Order Name")
		.WithDescription("Retrieves a paginated list of orders filtered by order name")
		.Produces<GetOrdersByOrderNameResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound);
	}
}

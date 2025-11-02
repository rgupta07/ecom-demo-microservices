using Ordering.Application.Ordering.Queries.GetAllOrders;

namespace Ordering.API.Endpoints.GetOrders;

public record GetAllOrdersRequest: PaginationRequest;
public record GetAllOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetAllOrdersEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/orders", async ([AsParameters] GetAllOrdersRequest request, IMediator mediator, CancellationToken cancellationToken) =>
		{
			var query = new GetAllOrdersQuery(request.PageNumber, request.PageSize);

			var result = await mediator.Send(query, cancellationToken);

			var response = result.Adapt<GetAllOrdersResponse>();

			return response;
		})
		.WithName("GetAllOrders")
		.WithSummary("Get All Orders")
		.WithDescription("Retrieves a paginated list of all orders")
		.Produces<GetAllOrdersResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest);
	}
}

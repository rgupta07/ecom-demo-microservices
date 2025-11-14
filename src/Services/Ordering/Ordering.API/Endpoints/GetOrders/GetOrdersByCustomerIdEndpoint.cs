using BuildingBlocks.Pagination;
using Ordering.Application.Ordering.Queries.GetOrdersByCustomerId;

namespace Ordering.API.Endpoints.GetOrders;

public record GetOrdersByCustomerIdRequest: PaginationRequest
{
	public Guid CustomerId { get; set; }
}

public record GetOrdersByCustomerIdResponse(PaginatedResult<OrderDto> Orders);

public class GetOrdersByCustomerIdEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/orders/by-customer-id/{customerId}", async ([AsParameters] GetOrdersByCustomerIdRequest request, IMediator mediator, CancellationToken cancellationToken) =>
		{
			var query = new GetOrderByCustomerIdQuery(request.CustomerId, request.PageNumber, request.PageSize);

			var result = await mediator.Send(query, cancellationToken);

			var response = result.Adapt<GetOrdersByCustomerIdResponse>();

			return response;
		})
		.WithName("GetOrdersByCustomerId")
		.WithSummary("Get Orders By Customer Id")
		.WithDescription("Retrieves a paginated list of orders for a specific customer")
		.Produces<GetOrdersByCustomerIdResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest);
	}
}

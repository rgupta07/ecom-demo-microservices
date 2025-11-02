namespace Ordering.Application.Ordering.Queries.GetOrdersByCustomerId;

public record GetOrderByCustomerIdQuery(Guid CustomerId, int PageNumber, int PageSize)
	: PaginationRequest(PageNumber, PageSize), IQuery<GetOrderByCustomerIdResult>;

public record GetOrderByCustomerIdResult(PaginatedResult<OrderDto> Result);

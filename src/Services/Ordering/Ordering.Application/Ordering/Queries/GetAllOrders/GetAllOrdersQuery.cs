namespace Ordering.Application.Ordering.Queries.GetAllOrders;

public record GetAllOrdersQuery(int PageNumber, int PageSize) : 
	PaginationRequest(PageNumber, PageSize), IQuery<GetAllOrdersResult>;

public record GetAllOrdersResult(PaginatedResult<OrderDto> Orders);	

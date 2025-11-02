namespace Ordering.Application.Ordering.Queries.GetOrdersByOrderName;

public record GetOrdersByOrderNameQuery(string OrderName, int PageNumber, int PageSize): 
	PaginationRequest(PageNumber, PageSize), IQuery<GetOrdersByOrderNameResult>;

public record GetOrdersByOrderNameResult(PaginatedResult<OrderDto> Orders);



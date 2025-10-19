
using BuildingBlocks.Pagination;

namespace Ordering.Application.Ordering.Queries.GetOrdersByCustomerId;
public record GetOrderByCustomerIdQuery(Guid CustomerId): PaginationRequest, IQuery<GetOrderByCustomerIdResult>;

public record GetOrderByCustomerIdResult(PaginatedResult<OrderDto> result);

using Mapster;

namespace Ordering.Application.Ordering.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetAllOrdersQuery, GetAllOrdersResult>
{
	public async Task<GetAllOrdersResult> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
	{
		var orders = await  dbContext.Orders
			.Include(o => o.OrderItems)
			.AsNoTracking()
			.OrderBy(o => o.Id)
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var totalCount = await dbContext.Orders.CountAsync(cancellationToken);
		
		var ordersDto = orders.Adapt<List<OrderDto>>();

		var result = new GetAllOrdersResult(
				new PaginatedResult<OrderDto>(
					request.PageNumber,
					request.PageSize,
					totalCount,
					ordersDto
				)
			);

		return result;
	}
}

public class GetAllOrdersQueryHandlerValidator: AbstractValidator<GetAllOrdersQuery>
{
	public GetAllOrdersQueryHandlerValidator()
	{
		RuleFor(x => x.PageNumber)
			.GreaterThan(0).WithMessage("Page number must be greater than 0.");
		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
	}
}

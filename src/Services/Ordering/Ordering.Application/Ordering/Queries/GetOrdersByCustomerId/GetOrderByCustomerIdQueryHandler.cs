namespace Ordering.Application.Ordering.Queries.GetOrdersByCustomerId;

public class GetOrderByCustomerIdQueryHandler(IApplicationDbContext _dbContext) : IQueryHandler<GetOrderByCustomerIdQuery, GetOrderByCustomerIdResult>
{
	public async Task<GetOrderByCustomerIdResult> Handle(GetOrderByCustomerIdQuery request, CancellationToken cancellationToken)
	 {
		var orders = await _dbContext.Orders
			.Include(o => o.OrderItems)
			.AsNoTracking()
			.Where(o => o.CustomerId == CustomerId.Of(request.CustomerId))
			.OrderBy(o => o.OrderName.Value)
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var ordersCount = orders.Count;

		var ordersDto = orders.Adapt<List<OrderDto>>();

		return new GetOrderByCustomerIdResult(
			new PaginatedResult<OrderDto>(
				request.PageNumber, request.PageSize, ordersCount, ordersDto)
		);
	}
}

public class GetOrderByCustomerIdQueryHandlerValidator: AbstractValidator<GetOrderByCustomerIdQuery>
{
	public GetOrderByCustomerIdQueryHandlerValidator()
	{
		RuleFor(x => x.CustomerId)
			.NotEmpty().WithMessage("CustomerId cannot be empty!");
	}
}

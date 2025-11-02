namespace Ordering.Application.Ordering.Queries.GetOrdersByOrderName;

public class GetOrdersByOrderNameQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByOrderNameQuery, GetOrdersByOrderNameResult>
{
	public async Task<GetOrdersByOrderNameResult> Handle(GetOrdersByOrderNameQuery request, CancellationToken cancellationToken)
	{
		var orders = await dbContext.Orders
			.AsNoTracking()
			.Include(o => o.OrderItems)
			.Where(x => x.OrderName == OrderName.Of(request.OrderName))
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var totalCount = await dbContext.Orders
			.AsNoTracking()
			.CountAsync(x => x.OrderName.Value == request.OrderName, cancellationToken);

		var ordersDto = orders.Adapt<IEnumerable<OrderDto>>();

		return new GetOrdersByOrderNameResult(
			new PaginatedResult<OrderDto>
			(
				request.PageNumber,
				request.PageSize,
				totalCount,
				ordersDto
			)
		);
	}
}


public class GetOrdersByOrderNameQueryHandlerValidator: AbstractValidator<GetOrdersByOrderNameQuery>
{
	public GetOrdersByOrderNameQueryHandlerValidator()
	{
		RuleFor(x => x.OrderName)
			.NotNull().NotEmpty()
			.WithMessage("OrderName cannot be null or empty");
		RuleFor(x => x.PageNumber)
			.GreaterThan(0).WithMessage("Page number must be greater than 0.");
		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
	}
}
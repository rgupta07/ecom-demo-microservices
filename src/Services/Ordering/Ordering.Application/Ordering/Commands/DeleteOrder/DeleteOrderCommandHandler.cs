
using Ordering.Application.Exceptions;

namespace Ordering.Application.Ordering.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IApplicationDbContext _dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
	public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _dbContext.Orders.FindAsync(OrderId.Of(request.OrderId), cancellationToken) ?? throw new OrderNotFoundException(request.OrderId);

		_dbContext.Orders.Remove(order);

		await _dbContext.SaveChangesAsync(cancellationToken);

		return new DeleteOrderResult(true);
	}
}

public class DeleteOrderCommandHandlerValidator: AbstractValidator<DeleteOrderCommand>
{
	public DeleteOrderCommandHandlerValidator()
	{
		RuleFor(x => x.OrderId)
			.NotNull().NotEmpty()
			.WithMessage("OrderId cannot be null be null or empty!");
	}
}
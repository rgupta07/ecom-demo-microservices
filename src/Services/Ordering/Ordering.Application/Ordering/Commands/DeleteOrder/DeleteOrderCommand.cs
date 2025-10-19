namespace Ordering.Application.Ordering.Commands.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId): ICommand<DeleteOrderResult>;

public record DeleteOrderResult();
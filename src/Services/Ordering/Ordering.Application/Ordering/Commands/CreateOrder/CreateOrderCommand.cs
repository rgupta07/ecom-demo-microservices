namespace Ordering.Application.Ordering.Commands.CreateOrder;
public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid OrderId);
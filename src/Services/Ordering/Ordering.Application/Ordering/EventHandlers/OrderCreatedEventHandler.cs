namespace Ordering.Application.Ordering.EventHandlers;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
	: INotificationHandler<OrderCreatedEvent>
{
	public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
	{
		logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

		await Task.CompletedTask;
	}
}

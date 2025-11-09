using MassTransit;

namespace Ordering.Application.Ordering.EventHandlers.Domain;
public class OrderUpdatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<OrderUpdatedEventHandler> logger)
	: INotificationHandler<OrderUpdatedEvent>
{
	public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
	{
		logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);

		var orderUpdatedIntegrationEvent = notification.Order.Adapt<OrderDto>();

		await publishEndpoint.Publish(orderUpdatedIntegrationEvent, cancellationToken);
	}
}

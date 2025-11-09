
using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Ordering.Commands.CreateOrder;

namespace Ordering.Application.Ordering.EventHandlers.Integration;

public class ShoppingCartCheckoutEventHandler(ISender sender, ILogger<ShoppingCartCheckoutEventHandler> logger) : IConsumer<BasketCheckoutEvent>
{
	public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
	{
		logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

		var command = context.Message.Adapt<CreateOrderCommand>();

		await sender.Send(command);
	}
}

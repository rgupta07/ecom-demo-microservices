using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BuildingBlocks.Behaviours;
using BuildingBlocks.Messaging.Events;
using Ordering.Application.Ordering.Commands.CreateOrder;
using BuildingBlocks.Messaging.Extensions;
using Microsoft.FeatureManagement;
using Ordering.Domain.Enums;

namespace Ordering.Application.Extensions;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplicationServices
		(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddLogging();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddMappingConfiguration();

		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
			config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
		});

		services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

		services.AddFeatureManagement(configuration);

		return services;
	}

	public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
	{
		var config = TypeAdapterConfig.GlobalSettings;

		config.NewConfig<BasketCheckoutEvent, CreateOrderCommand>()
			.MapWith(src => new CreateOrderCommand(
				new OrderDto(
					Guid.Empty, // Id will be generated in the handler
					src.CustomerId,
					src.UserName,
					new AddressDto(
						src.ShippingAddressFirstName,
						src.ShippingAddressLastName,
						src.ShippingAddressEmailAddress,
						src.ShippingAddressLine,
						src.ShippingAddressCountry,
						src.ShippingAddressState,
						src.ShippingAddressZipCode
					),
					new AddressDto(
						src.BillingAddressFirstName,
						src.BillingAddressLastName,
						src.BillingAddressEmailAddress,
						src.BillingAddressLine,
						src.BillingAddressCountry,
						src.BillingAddressState,
						src.BillingAddressZipCode
					),
					new PaymentDto(
						src.CardName,
						src.CardNumber,
						src.Expiration,
						src.CVV,
						(PaymentMethod)src.PaymentMethod
					),
					OrderStatus.Pending, // Default status
					src.OrderItems.Select(item => new OrderItemDto(
						Guid.Empty, // OrderId will be set later
						item.ProductId,
						item.Quantity,
						item.Price
					)).ToList()
				)
			));

		return services;
	}
}

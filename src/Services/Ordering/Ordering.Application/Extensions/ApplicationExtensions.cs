using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BuildingBlocks.Behaviours;
using BuildingBlocks.Messaging.Events;
using Ordering.Application.Ordering.Commands.CreateOrder;

namespace Ordering.Application.Extensions;
public static class ApplicationExtensions
{
	public static IServiceCollection AddApplicationServices
		(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddLogging();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
			config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
		});

		return services;
	}

	public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
	{
		TypeAdapterConfig<BasketCheckoutEvent, CreateOrderCommand>
			.NewConfig()
			.Map(dest => dest.Order.CustomerId, src => src.CustomerId)
			.Map(dest => dest.Order.OrderName, src => src.UserName)
			// Map Shipping Address
			.Map(dest => dest.Order.ShippingAddress.FirstName, src => src.ShippingAddressFirstName)
			.Map(dest => dest.Order.ShippingAddress.LastName, src => src.ShippingAddressLastName)
			.Map(dest => dest.Order.ShippingAddress.EmailAddress, src => src.ShippingAddressEmailAddress)
			.Map(dest => dest.Order.ShippingAddress.AddressLine, src => src.ShippingAddressLine)
			.Map(dest => dest.Order.ShippingAddress.Country, src => src.ShippingAddressCountry)
			.Map(dest => dest.Order.ShippingAddress.State, src => src.ShippingAddressState)
			.Map(dest => dest.Order.ShippingAddress.ZipCode, src => src.ShippingAddressZipCode)
			// Map Billing Address
			.Map(dest => dest.Order.BillingAddress.FirstName, src => src.BillingAddressFirstName)
			.Map(dest => dest.Order.BillingAddress.LastName, src => src.BillingAddressLastName)
			.Map(dest => dest.Order.BillingAddress.EmailAddress, src => src.BillingAddressEmailAddress)
			.Map(dest => dest.Order.BillingAddress.AddressLine, src => src.BillingAddressLine)
			.Map(dest => dest.Order.BillingAddress.Country, src => src.BillingAddressCountry)
			.Map(dest => dest.Order.BillingAddress.State, src => src.BillingAddressState)
			.Map(dest => dest.Order.BillingAddress.ZipCode, src => src.BillingAddressZipCode)
			//Map Payment
			.Map(dest => dest.Order.Payment.CardName, src => src.CardName)
			.Map(dest => dest.Order.Payment.CardNumber, src => src.CardNumber)
			.Map(dest => dest.Order.Payment.Expiration, src => src.Expiration)
			.Map(dest => dest.Order.Payment.Cvv, src => src.CVV)
			.Map(dest => dest.Order.Payment.PaymentMethod, src => src.PaymentMethod)
			//Map Order Items
			.Map(dest => dest.Order.OrderItems, src => new List<OrderItemDto>()
			{
				new(guid)
			}

		return services;
	}
}

using Discount.gRPC.Protos;
using BuildingBlocks.Messaging.Extensions;
using System.Reflection;
using Basket.API.DTOs;
using BuildingBlocks.Messaging.Events;
using Basket.API.ShoppingCartFeature.CheckoutShoppingCartFeature;

namespace Basket.API.Extensions;

public static class BasketApiExtensions
{
	public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
	{
		services.AddValidatorsFromAssembly(assembly);

		services.AddCarter();

		services.AddScoped<IShopingCartRepository, ShoppingCartRepository>();
		services.Decorate<IShopingCartRepository, CachedShoppingCartRepository>();

		services.AddStackExchangeRedisCache(opts =>
		{
			opts.Configuration = configuration.GetConnectionString("RedisCacheConnection");
		});

		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(assembly);
			config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
			config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
		});

		services.AddMarten(builder =>
		{
			builder.Connection(configuration.GetConnectionString("BasketDBConnection")!);
			builder.Schema.For<ShoppingCart>().Identity(x => x.UserName);
			builder.AutoCreateSchemaObjects = AutoCreate.All;
		}).UseLightweightSessions();

		services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o =>
		{
			o.Address = new Uri(configuration.GetValue<string>("GrpcSettings:DiscountUrl")!);
		})
		.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
		{
			// Return `true` to allow certificates that are untrusted/invalid
			ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
		});

		services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o =>
		{
			o.Address = new Uri(configuration.GetValue<string>("GrpcSettings:DiscountUrl")!);
		})
		.ConfigurePrimaryHttpMessageHandler(() =>
		{
			var handler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback =
				HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};

			return handler;
		});

		services.AddMessageBroker(configuration);

		return services;
	}


	public static IServiceCollection AddMapping(this IServiceCollection services)
	{
		TypeAdapterConfig<ShoppingCartCheckoutDto, BasketCheckoutEvent>
			.NewConfig()
			.Map(dest => dest.UserName, src => src.UserName)
			.Map(dest => dest.CustomerId, src => src.CustomerId)
			.Map(dest => dest.TotalPrice, src => src.TotalPrice)

			// Shipping Address
			.Map(dest => dest.ShippingAddressFirstName, src => src.ShippingAddressFirstName)
			.Map(dest => dest.ShippingAddressLastName, src => src.ShippingAddressLastName)
			.Map(dest => dest.ShippingAddressEmailAddress, src => src.ShippingAddressEmailAddress)
			.Map(dest => dest.ShippingAddressLine, src => src.ShippingAddressLine)
			.Map(dest => dest.ShippingAddressCountry, src => src.ShippingAddressCountry)
			.Map(dest => dest.ShippingAddressState, src => src.ShippingAddressState)
			.Map(dest => dest.ShippingAddressZipCode, src => src.ShippingAddressZipCode)

			// Billing Address
			.Map(dest => dest.BillingAddressFirstName, src => src.BillingAddressFirstName)
			.Map(dest => dest.BillingAddressLastName, src => src.BillingAddressLastName)
			.Map(dest => dest.BillingAddressEmailAddress, src => src.BillingAddressEmailAddress)
			.Map(dest => dest.BillingAddressLine, src => src.BillingAddressLine)
			.Map(dest => dest.BillingAddressCountry, src => src.BillingAddressCountry)
			.Map(dest => dest.BillingAddressState, src => src.BillingAddressState)
			.Map(dest => dest.BillingAddressZipCode, src => src.BillingAddressZipCode)

			// Order Items
			.Map(dest => dest.OrderItems, src => src.ShoppingCartItems == null
				? new List<OrderItem>()
				: src.ShoppingCartItems.Select(i => new OrderItem(Guid.NewGuid(), i.ProductId, i.Quantity, i.Price)).ToList())

			// Payment
			.Map(dest => dest.CardName, src => src.CardName)
			.Map(dest => dest.CardNumber, src => src.CardNumber)
			.Map(dest => dest.Expiration, src => src.Expiration)
			.Map(dest => dest.CVV, src => src.CVV)
			.Map(dest => dest.PaymentMethod, src => src.PaymentMethod);


		TypeAdapterConfig<CheckoutShoppingCartRequest, CheckoutShoppingCartCommand>
			.NewConfig()
			.Map(dest => dest.CartCheckoutDto, src => src.ShoppingCart);

		return services;
	}
}

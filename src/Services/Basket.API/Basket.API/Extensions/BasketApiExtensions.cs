using Discount.gRPC.Protos;
using BuildingBlocks.Messaging.Extensions;
using System.Reflection;
using Basket.API.DTOs;
using BuildingBlocks.Messaging.Events;

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
			.Map(dest => dest.OrderItems, src => src.ShoppingCartItems);

		return services;
	}
}

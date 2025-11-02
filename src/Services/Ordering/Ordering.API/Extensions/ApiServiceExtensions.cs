using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ordering.Domain.Entities;
using Ordering.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace Ordering.API.Extensions;

public static class ApiServiceExtensions
{
	public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddCarter();
		services.AddMappingConfiguration();

		services.ConfigureHttpJsonOptions(options =>
		{
			options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
		});

		services.AddHealthChecks()
			.AddSqlServer(configuration.GetConnectionString("Database")!);

		return services;
	}

	public static WebApplication UseApiServices(this WebApplication app)
	{
		app.MapCarter();

		app.UseExceptionHandler(opts => { });

		app.UseHealthChecks("/health",
			new HealthCheckOptions()
			{
				ResponseWriter = async (context, report) =>
				{
					context.Response.ContentType = "application/json";
					var result = System.Text.Json.JsonSerializer.Serialize(new
					{
						status = report.Status.ToString(),
						results = report.Entries.Select(e => new {
							key = e.Key,
							status = e.Value.Status.ToString(),
							description = e.Value.Description
						})
					});
					await context.Response.WriteAsync(result);
				}
			}
		);

		return app;
	}

	public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
	{
		TypeAdapterConfig<Order, OrderDto>
		.NewConfig()
		.Map(dest => dest.Id, src => src.Id.Value)
		.Map(dest => dest.CustomerId, src => src.CustomerId.Value)
		.Map(dest => dest.OrderName, src => src.OrderName.Value)
		.Map(dest => dest.ShippingAddress, src => src.ShippingAddress)
		.Map(dest => dest.BillingAddress, src => src.BillingAddress)
		.Map(dest => dest.Payment, src => src.Payment)
		.Map(dest => dest.Status, src => src.Status)
		.Map(dest => dest.OrderItems, src => src.OrderItems);

		TypeAdapterConfig<OrderItem, OrderItemDto>
			.NewConfig()
			.Map(dest => dest.OrderId, src => src.OrderId.Value)
			.Map(dest => dest.ProductId, src => src.ProductId.Value);

		TypeAdapterConfig<Address, AddressDto>
			.NewConfig()
			.Map(dest => dest, src => src); // Assuming Address has implicit properties mapping

		TypeAdapterConfig<Payment, PaymentDto>
			.NewConfig()
			.Map(dest => dest, src => src); // Assuming Payment has implicit properties mapping

		return services;
	}
}

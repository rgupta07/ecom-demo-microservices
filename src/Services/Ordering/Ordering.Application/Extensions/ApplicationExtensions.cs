using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BuildingBlocks.Behaviours;

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
}

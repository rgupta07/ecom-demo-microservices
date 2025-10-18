using Microsoft.AspNetCore.Http;
using Ordering.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Ordering.Infrastructure.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Extensions;
public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructureServices
		(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Database");

		// Add services to the container.
		services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
		services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
		services.AddHttpContextAccessor();

		services.AddDbContext<ApplicationDbContext>((sp, options) =>
		{
			options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
			options.UseSqlServer(connectionString);
		});

		services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

		return services;
	}
}

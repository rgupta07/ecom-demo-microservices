using Microsoft.AspNetCore.Builder;
using Ordering.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data.Setup;
using Ordering.Infrastructure.Utilities.Faker;


namespace Ordering.Infrastructure.Extensions;
public static class DatabaseExtentions
{
	public static async Task InitialiseDatabaseAsync(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		await context.Database.MigrateAsync();

		await SeedAsync(context);
	}

	private static async Task SeedAsync(ApplicationDbContext context)
	{
		await SeedCustomerAsync(context);
		await SeedProductAsync(context);
		await SeedOrdersWithItemsAsync(context);
	}

	private static async Task SeedCustomerAsync(ApplicationDbContext context)
	{
		if (!await context.Customers.AnyAsync())
		{
			await context.Customers.AddRangeAsync(CustomerFaker.GenerateCustomers());
			await context.SaveChangesAsync();
		}
	}

	private static async Task SeedProductAsync(ApplicationDbContext context)
	{
		if (!await context.Products.AnyAsync())
		{
			await context.Products.AddRangeAsync(ProductFaker.GenerateProducts());
			await context.SaveChangesAsync();
		}
	}

	private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
	{
		if (!await context.Orders.AnyAsync())
		{
			await context.Orders.AddRangeAsync(OrdersFaker.GenerateOrders());
			await context.SaveChangesAsync();
		}
	}
}

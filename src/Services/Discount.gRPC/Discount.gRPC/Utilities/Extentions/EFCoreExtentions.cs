namespace Discount.gRPC.Utilities.Extentions
{
	public static class EFCoreExtentions
	{
		public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder builder)
		{
			using var scope = builder.ApplicationServices.CreateScope();

			var services = scope.ServiceProvider;

			var context = services.GetRequiredService<DiscountDbContext>();

			context.Database.Migrate();

			return builder;
		}
	}
}

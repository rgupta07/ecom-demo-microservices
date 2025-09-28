namespace Discount.gRPC.Utilities.Extentions
{
	public static class EFCoreExtentions
	{
		public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder builder)
		{
			using var scope = builder.ApplicationServices.CreateScope();
			var services = scope.ServiceProvider;
			var context = services.GetRequiredService<DiscountDbContext>();

			// Check if the Coupon table exists
			var tableExists = context.Database.ExecuteSqlRaw(
				"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Coupon';"
			) > 0;

			if (!tableExists)
			{
				context.Database.Migrate();
				return builder;
			}

			if (context.Coupon.Any())
			{
				return builder;
			}

			context.Database.Migrate();
			return builder;
		}
	}
}

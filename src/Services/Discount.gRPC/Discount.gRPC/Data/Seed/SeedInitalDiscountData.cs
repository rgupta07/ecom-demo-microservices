using Discount.gRPC.Utilities.Faker;

namespace Discount.gRPC.Data.Seed
{
	public static class SeedInitalDiscountData
	{
		public static int SEED_COUNT = 100;
		
		public static IList<Coupon> SeedData()
		{
			var discounts = new List<Coupon>();

			int count = 0;

			do
			{
				var discount = new DiscountFaker().Generate();
				discount.Id = count + 1;
				discounts.Add(discount);
				count++;
			}
			while(count < SEED_COUNT);

			return discounts;
		}
	}
}

using Bogus;

namespace Discount.gRPC.Utilities.Faker
{
	public class DiscountFaker : Faker<Coupon>
	{
		public DiscountFaker()
		{
			RuleFor(c => c.ProductName, (f, c) => f.Commerce.ProductName());
			RuleFor(c => c.Description, (f, c) => f.Commerce.ProductDescription());
			RuleFor(c => c.DiscountAmount, (f, c) => decimal.Parse(f.Commerce.Price(1, 100, 2)));
		}
	}
}

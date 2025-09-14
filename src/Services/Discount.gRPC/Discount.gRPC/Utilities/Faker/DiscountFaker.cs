using Bogus;

namespace Discount.gRPC.Utilities.Faker
{
	public class DiscountFaker: Faker<Coupon>
	{
		public DiscountFaker()
		{
			RuleFor(c => c.ProductName, f => f.Commerce.ProductName());
			RuleFor(c => c.Description, f => f.Commerce.ProductAdjective());
			RuleFor(c => c.DiscountAmount, f => decimal.Parse(f.Commerce.Price(1, 100)));
		}
	}
}

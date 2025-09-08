using Bogus;

namespace Basket.API.Utilities.Faker
{
	public class ShoppingCartFaker: Faker<ShoppingCart>
	{
		public ShoppingCartFaker()
		{
			RuleFor(x => x.UserName, f => f.Internet.UserName());
			RuleFor(x => x.Items, f =>
			{
				var itemFaker = new ShoppingCartItemFaker();
				return itemFaker.Generate(f.Random.Int(1, 5));
			});
		}
	}

	public class ShoppingCartItemFaker : Faker<ShoppingCartItem>
	{
		public ShoppingCartItemFaker()
		{
			RuleFor(x => x.ProductId, (f) => Guid.NewGuid());
			RuleFor(x => x.ProductName, f => f.Commerce.ProductName());
			RuleFor(x => x.Quantity, f => f.Random.Int(1, 10));
			RuleFor(x => x.Price, f => decimal.Parse(f.Commerce.Price(1, 100)));
			RuleFor(x => x.Color, f => f.Commerce.Color());
		}
	}
}

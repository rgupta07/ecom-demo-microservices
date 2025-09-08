using Basket.API.Utilities.Faker;
using Marten.Schema;

namespace Basket.API.Data
{
	public class BasketInitialDataSetup(ILogger<BasketInitialDataSetup> logger) : IInitialData
	{
		public const int SEED_COUNT = 100;
		public async Task Populate(IDocumentStore store, CancellationToken cancellation)
		{
			logger.LogInformation("Initializing seeding of mock data for BasketDb.");

			var session = store.LightweightSession();

			var cartItems = session.Query<ShoppingCart>().ToList();

			if(cartItems.Count > 0)
			{
				logger.LogInformation("BasketDb already has data. skipping seeding");
				return;
			}

			var shoppingCarts = GenerateShoppingCartData();

			session.Store<ShoppingCart>(shoppingCarts);

			await session.SaveChangesAsync();

			logger.LogInformation("Completed seeding of mock data for BasketDb.");

		}

		public IList<ShoppingCart> GenerateShoppingCartData()
		{
			int count = 0;

			var shoppingCarts = new List<ShoppingCart>();

			do
			{
				var shoopingcart = new ShoppingCartFaker().Generate();
				shoppingCarts.Add(shoopingcart);
				count++;
			}
			while (count < SEED_COUNT);

			return shoppingCarts;
		}

	}

}

using Catalog.API.Models;
using Catalog.API.Utilities.Faker;
using Marten.Schema;
using Weasel.Postgresql.Tables.Partitioning;

namespace Catalog.API.Data
{
	public class CatalogInitialDataSetup(ILogger<CatalogInitialDataSetup> logger) : IInitialData
	{
		private const int SEED_COUNT = 100;
		public async Task Populate(IDocumentStore store, CancellationToken cancellation)
		{
			logger.LogInformation("Initiating seeding of Product objects for CatalogDB");

			var session = store.LightweightSession();

			var products = session.Query<Product>().Count();

			if(products > 0)
			{
				logger.LogInformation("Catalog DB already has Product objects, skipping population.");
				return;
			}

			var seededProducts = GenerateInitialProductData();

			session.Store<Product>(seededProducts);

			await session.SaveChangesAsync(cancellation);

			logger.LogInformation("Seeding of Product objects for CatalogDB completed successfully.");
		}

		private static IEnumerable<Product> GenerateInitialProductData()
		{
			int count = 0;
			var products = new List<Product>();

			do
			{
				var product = new ProductFaker().Generate();
				products.Add(product);
				count++;
			}
			while (count < SEED_COUNT);

			return products;
		}
	}
}

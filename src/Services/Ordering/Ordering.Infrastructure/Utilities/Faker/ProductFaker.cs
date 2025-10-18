using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Utilities.Faker;
public class ProductFaker: Faker<Product>
{
	private static readonly int PRODUCTS_COUNT = 100;

	private static readonly List<Product> _products = [];
	public ProductFaker()
	{
		CustomInstantiator(f => Product.Create(
			ProductId.Of(Guid.NewGuid()),
			f.Commerce.ProductName(),
			decimal.Parse(f.Commerce.Price(1, 1000))
		));
	}

	public static List<Product> GenerateProducts()
	{
		if(_products.Count > 0) return _products;
		var faker = new ProductFaker();
		_products.AddRange(faker.Generate(PRODUCTS_COUNT));
		return _products;
	}

	public static Product GetRandomProduct()
	{
		if(_products.Count == 0)
		{
			_products.AddRange(GenerateProducts());
		}
		var random = new Random();
		int index = random.Next(_products.Count);
		return _products[index];
	}
}

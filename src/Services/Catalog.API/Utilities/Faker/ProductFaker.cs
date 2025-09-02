using Catalog.API.Models;

namespace Catalog.API.Utilities.Faker
{
	public class ProductFaker : Faker<Product>
	{
		public ProductFaker()
		{
			RuleFor(p => p.Id, (f) => Guid.NewGuid());
			RuleFor(p => p.Name, (f) => f.Commerce.Product());
			RuleFor(p => p.Description, (f) => f.Commerce.ProductDescription());
			RuleFor(p => p.Category, (f) => f.Commerce.Categories(Random.Shared.Next(2, 10)).ToList());
			RuleFor(p => p.ImgFile, (f) => f.Image.PicsumUrl());
			RuleFor(p => p.Price, (f) => decimal.Parse(f.Commerce.Price(10, 100, 2)));
		}
	}
}

using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions
{
	public class ProductNotFoundException: NotFoundException
	{
		public ProductNotFoundException(): base() { }
		public ProductNotFoundException(Guid id) : base("Product Id", id) { }
	}
}

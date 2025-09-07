using BuildingBlocks.Exceptions;

namespace Basket.API.Exceptions
{
	public class CartNotFoundException: NotFoundException
	{
		public CartNotFoundException() : base()
		{
		}
		public CartNotFoundException(string userName) : base("Shopping Cart for", userName)
		{
		}
	}
}

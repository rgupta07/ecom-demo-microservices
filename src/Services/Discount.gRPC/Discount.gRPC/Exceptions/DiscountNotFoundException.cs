using BuildingBlocks.Exceptions;

namespace Discount.gRPC.Exceptions
{
	public class DiscountNotFoundException: NotFoundException
	{
		public DiscountNotFoundException(string productName)
			: base("Discount", productName)
		{
		}
	}
}

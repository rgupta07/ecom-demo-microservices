using BuildingBlocks.Exceptions;

namespace Ordering.Application.Exceptions;
public class OrderNotFoundException: NotFoundException
{
	public OrderNotFoundException(): base() { }

	public OrderNotFoundException(object key) : base("Order", key)
	{
	}

	public OrderNotFoundException(object key, string selector) : base($"Order", key, selector)
	{
	}
}

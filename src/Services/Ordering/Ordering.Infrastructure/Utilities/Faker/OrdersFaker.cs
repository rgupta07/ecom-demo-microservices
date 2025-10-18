using Bogus;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Utilities.Faker;
public class OrdersFaker: Faker<Order>
{
	private static readonly int ORDERS_COUNT = 500;
	private static readonly List<Order> _orders = [];
	public OrdersFaker()
	{
		CustomInstantiator(f =>
		{
			var customerId = CustomerFaker.GetRandomCustomer().Id;
			var orderName = OrderName.Of(f.Commerce.ProductName());
			
			var shippingAddress = Address.Of(
				f.Name.FirstName(),
				f.Name.LastName(),
				f.Internet.Email(),
				f.Address.StreetAddress(),
				f.Address.City(),
				f.Address.State(),
				f.Address.ZipCode("?????"));

			var billingAddress = shippingAddress;

			var payment = Payment.Of(
				f.Person.LastName,
				f.Finance.CreditCardNumber(),
				f.Date.Future().ToShortDateString(),
				f.Finance.CreditCardCvv(),
				f.PickRandom<PaymentMethod>());

			var numberOfItems = f.Random.Int(1, 5);
			var orderItems = new List<OrderItem>();
			
			// Create a temporary OrderId for the order items
			var tempOrderId = OrderId.Of(Guid.NewGuid());
			
			for (int i = 0; i < numberOfItems; i++)
			{
				var product = ProductFaker.GetRandomProduct();
				var quantity = f.Random.Int(1, 10);
				var price = product.Price;
				var orderItem = OrderItem.Create(tempOrderId, product.Id, quantity, price);
				orderItems.Add(orderItem);
			}

			var order = Order.Create(customerId, orderName, shippingAddress, billingAddress, payment, orderItems);
			
			// Set the status if needed (after creation)
			var status = f.PickRandom<OrderStatus>();
			if (status != OrderStatus.Pending)
			{
				order.Update(orderName, shippingAddress, billingAddress, payment, status);
			}
			
			return order;
		});
	}

	public static List<Order> GenerateOrders()
	{
		var ordersFaker = new OrdersFaker();
		_orders.AddRange(ordersFaker.Generate(ORDERS_COUNT));
		return _orders;
	}

}

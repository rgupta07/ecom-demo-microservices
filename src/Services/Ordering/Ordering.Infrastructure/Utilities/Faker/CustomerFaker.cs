using Bogus;

namespace Ordering.Infrastructure.Utilities.Faker;
public class CustomerFaker : Faker<Customer>
{
	private static readonly int CUSTOMERS_COUNT = 100;

	private static readonly List<Customer> _customers = [];
	public CustomerFaker()
	{
		CustomInstantiator(f => Customer.Create(
			CustomerId.Of(f.Random.Guid()),
			f.Person.FullName,
			f.Internet.Email()
		));
	}

	public static IList<Customer> GenerateCustomers()
	{
		if(_customers.Count > 0) return _customers;
		var faker = new CustomerFaker();
		_customers.AddRange(faker.Generate(CUSTOMERS_COUNT));
		return _customers;
	}

	public static Customer GetRandomCustomer()
	{
		if (_customers.Count == 0)
		{
			GenerateCustomers();
		}
		var random = new Random();
		int index = random.Next(_customers.Count);
		return _customers[index];
	}
}

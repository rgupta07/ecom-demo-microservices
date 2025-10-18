using Ordering.Infrastructure.Utilities.Faker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Setup;
public class OrderDbInitialDataSetup
{
	public static void SetupInitialData()
	{
		//Generate the customers
		CustomerFaker.GenerateCustomers();
		//Generate the products
		ProductFaker.GenerateProducts();
		//Generate the orders
		OrdersFaker.GenerateOrders();
	}
}

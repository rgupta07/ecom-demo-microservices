using Marten.Schema;

namespace Basket.API.Models
{
	public class ShoppingCart
	{
		[Identity]
		public string UserName { get; set; }
		public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
		public ShoppingCart() { }
		public ShoppingCart(string username)
		{
			UserName = username;
		}
		public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
	}
}

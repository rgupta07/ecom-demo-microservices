using Basket.API.Models;
using Basket.API.ShoppingCartFeature.DeleteShoppingCart;
using Basket.API.ShoppingCartFeature.GetShoppingCart;
using Basket.API.ShoppingCartFeature.UpdateShoppingCartFeature;

namespace Basket.API.Repository
{
	public interface IShopingCartRepository
	{
		Task<GetShoppingCartResult> GetShoppingCart(string userName);
		Task<UpdateShoppingCartResult> UpdateShoppingCart(ShoppingCart shoppingCart);
		Task<DeleteShoppingCartResult> DeleteShoppingCart(string userName);
	}
}

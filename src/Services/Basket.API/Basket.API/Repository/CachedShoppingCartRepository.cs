using Basket.API.ShoppingCartFeature.DeleteShoppingCart;
using Basket.API.ShoppingCartFeature.GetShoppingCart;
using Basket.API.ShoppingCartFeature.UpdateShoppingCartFeature;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repository
{
	public class CachedShoppingCartRepository(IShopingCartRepository cartRepository, IDistributedCache cache) : IShopingCartRepository
	{
		public async Task<GetShoppingCartResult> GetShoppingCart(string userName)
		{
			var cachedCart = await cache.GetStringAsync(userName);

			if(!string.IsNullOrEmpty(cachedCart))
			{
				return JsonSerializer.Deserialize<GetShoppingCartResult>(cachedCart)!;
			}
			// If not in cache, get from the underlying repository
			var result = await cartRepository.GetShoppingCart(userName);
			var cartJson = JsonSerializer.Serialize(result);
			await cache.SetStringAsync(userName, cartJson);
			return result;
		}
		public async Task<UpdateShoppingCartResult> UpdateShoppingCart(ShoppingCart shoppingCart)
		{
			await cartRepository.UpdateShoppingCart(shoppingCart);

			var result = await cartRepository.GetShoppingCart(shoppingCart.UserName);

			await cache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(result));

			return new(true);
		}
		public async Task<DeleteShoppingCartResult> DeleteShoppingCart(string userName)
		{
			await cartRepository.DeleteShoppingCart(userName);

			cache.Remove(userName);

			return new(true);
		}
	}
}

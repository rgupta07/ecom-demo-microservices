using Basket.API.Exceptions;
using Basket.API.Models;
using Basket.API.ShoppingCartFeature.DeleteShoppingCart;
using Basket.API.ShoppingCartFeature.GetShoppingCart;
using Basket.API.ShoppingCartFeature.UpdateShoppingCartFeature;
using Marten;

namespace Basket.API.Repository
{
	public class ShoppingCartRepository(IDocumentSession session) : IShopingCartRepository
	{
		public async Task<GetShoppingCartResult> GetShoppingCart(string userName)
		{
			var result = await session.Query<ShoppingCart>()
				.FirstOrDefaultAsync(cart => cart.UserName == userName);

			if (result != null)
			{
				return new GetShoppingCartResult(result);
			}

			return new GetShoppingCartResult(new ShoppingCart(userName));
		}

		public async Task<UpdateShoppingCartResult> UpdateShoppingCart(ShoppingCart shoppingCart)
		{
			var existingCart = session.Query<ShoppingCart>()
				.FirstOrDefault(cart => cart.UserName == shoppingCart.UserName);

			if (existingCart != null)
			{
				existingCart.Items = shoppingCart.Items;
				session.Update(existingCart);
				await session.SaveChangesAsync();
				
				return new UpdateShoppingCartResult(true);
			}

			return new UpdateShoppingCartResult(false);
		}

		public async Task<DeleteShoppingCartResult> DeleteShoppingCart(string userName)
		{
			var existingCart = session.Query<ShoppingCart>()
				.FirstOrDefault(cart => cart.UserName == userName);

			if( existingCart != null)
			{
				session.Delete(existingCart);
				await session.SaveChangesAsync();
				return new DeleteShoppingCartResult(true);
			}
			else
			{
				throw new CartNotFoundException(userName);
			}
		}
	}
}

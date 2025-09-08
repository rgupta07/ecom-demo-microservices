
using Basket.API.ShoppingCartFeature.DeleteShoppingCart;
using Basket.API.ShoppingCartFeature.GetShoppingCart;
using Basket.API.ShoppingCartFeature.UpdateShoppingCartFeature;

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

			throw new CartNotFoundException(userName);
		}

		public async Task<UpdateShoppingCartResult> UpdateShoppingCart(ShoppingCart shoppingCart)
		{
			var existingCart = session.Query<ShoppingCart>()
				.FirstOrDefault(cart => cart.UserName == shoppingCart.UserName);

			if (existingCart != null)
			{
				existingCart.Items = shoppingCart.Items;
				session.Update(existingCart);
			}
			else
			{
				session.Store(shoppingCart);
			}
			await session.SaveChangesAsync();
			return new UpdateShoppingCartResult(true);
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
			throw new CartNotFoundException(userName);
		}
	}
}

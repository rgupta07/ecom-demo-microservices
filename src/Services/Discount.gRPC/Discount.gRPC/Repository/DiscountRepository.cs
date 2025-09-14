using Discount.gRPC.Data.DbContexts;
using Discount.gRPC.Exceptions;
using Discount.gRPC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Discount.gRPC.Repository
{
	public class DiscountRepository(DiscountDbContext dbContext, ILogger<DiscountRepository> logger) : IDiscountRepository
	{
		public async Task<bool> CreateDiscount(Coupon coupon)
		{
			logger.LogInformation("Creating discount for product: {ProductName}", coupon.ProductName);

			dbContext.Coupon.Add(coupon);
		
			await dbContext.SaveChangesAsync();

			logger.LogInformation("Discount created for product: {ProductName}", coupon.ProductName);

			return true;
		}

		public async Task<Coupon> GetDiscount(string productName)
		{
			logger.LogInformation("Getting discount for product: {ProductName}", productName);

			var discount = await dbContext.Coupon.FirstOrDefaultAsync(c => c.ProductName == productName);
			
			if(discount == null)
				throw new DiscountNotFoundException(productName);

			logger.LogInformation("{ProductName} discount is retrieved successfully.", productName);

			return discount;
		}

		public async Task<bool> UpdateDiscount(Coupon coupon)
		{
			logger.LogInformation("Updating discount for product: {ProductName}", coupon.ProductName);

			var existingDiscount = await dbContext.Coupon.FirstOrDefaultAsync(c => c.ProductName == coupon.ProductName);

			if(existingDiscount == null)
				throw new DiscountNotFoundException(coupon.ProductName);

			existingDiscount.ProductName = coupon.ProductName;
			existingDiscount.Description = coupon.Description;
			existingDiscount.DiscountAmount = coupon.DiscountAmount;

			dbContext.Coupon.Update(existingDiscount);

			await dbContext.SaveChangesAsync();

			logger.LogInformation("Discount updated for product: {ProductName}", coupon.ProductName);

			return true;
		}

		public async Task<bool> DeleteDiscount(string productName)
		{
			logger.LogInformation("Deleting discount for product: {ProductName}", productName);

			var existingDiscount = await dbContext.Coupon.FirstOrDefaultAsync(c => c.ProductName == productName);

			if (existingDiscount == null)
				throw new DiscountNotFoundException(productName);

			dbContext.Coupon.Remove(existingDiscount);

			await dbContext.SaveChangesAsync();

			logger.LogInformation("{ProductName} discount is successfully deleted.", productName);

			return true;
		}
	}
}

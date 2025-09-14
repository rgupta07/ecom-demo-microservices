using Discount.gRPC.Data.Seed;
using Discount.gRPC.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Data.DbContexts
{
	public class DiscountDbContext: DbContext
	{
		public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Coupon>(entity =>
			{
				
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
			});

			modelBuilder.Entity<Coupon>().HasData(SeedInitalDiscountData.SeedData());
		}

		public DbSet<Coupon> Coupon { get; set; }
	}
}

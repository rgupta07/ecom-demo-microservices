using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Discount.gRPC.Models
{
	public class Coupon
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string ProductName { get; set; } = default!;
		public string Description { get; set; } = default!;
		public decimal DiscountAmount { get; set; }
	}
}

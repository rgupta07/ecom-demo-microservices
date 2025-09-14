using Discount.gRPC.Repository;
using Grpc.Core;
using Microsoft.AspNetCore.Components.Forms;

namespace Discount.gRPC.Services
{
	public class DiscountService(IDiscountRepository discountRepository): DiscountProtoService.DiscountProtoServiceBase
	{
		public override async Task<GetDiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var result = await discountRepository.GetDiscount(request.ProductName);

			var discountModel = result.Adapt<DiscountModel>();

			return new GetDiscountResponse { Discount = discountModel };
		}
		public override async Task<CreateDiscountResponse> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var req = request.Adapt<Coupon>();

			var result  = await discountRepository.CreateDiscount(req);

			return new CreateDiscountResponse { Success = result };
		}
		public override async Task<UpdateDiscountResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var req = request.Adapt<Coupon>();

			var result = await discountRepository.UpdateDiscount(req);

			return new UpdateDiscountResponse { Success = result };
		}
		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			var result = await discountRepository.DeleteDiscount(request.ProductName);

			return new DeleteDiscountResponse { Success = result };
		}

	}
}

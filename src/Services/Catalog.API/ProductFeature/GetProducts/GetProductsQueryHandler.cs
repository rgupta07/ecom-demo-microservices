using Catalog.API.Models;
using Marten.Pagination;

namespace Catalog.API.ProductFeature.GetProducts
{
	public record GetProductsQuery(int PageNumber = 1, int PageSize = 10): IQuery<GetProductsResult>;
	public record GetProductsResult(IEnumerable<Product> Products, long TotalCount);

	public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
	{
		public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
		{
			var products = await session.Query<Product>()
				.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

			return new GetProductsResult(products, products.TotalItemCount);
			throw new NotImplementedException();
		}
	}
}

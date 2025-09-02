using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.ProductFeature.GetProduct
{
	public record GetProductQuery(Guid Id) : IQuery<GetProductResult>;
	public record GetProductResult(string Name, string Description, IList<string> Categories, string ImgFile, decimal Price);
	public class GetProductQueryHandler(IDocumentSession session) : IQueryHandler<GetProductQuery, GetProductResult>
	{
		public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			var result = await session.LoadAsync<Product>(request.Id, cancellationToken);

			if (result == null)
			{
				throw new ProductNotFoundException(request.Id);
			}
			
			return new GetProductResult(
				result.Name,
				result.Description,
				result.Category,
				result.ImgFile,
				decimal.Parse(result.Price.ToString()));
		}
	}

	public class GetProductQueryHandlerValidator : AbstractValidator<GetProductQuery>
	{
		public GetProductQueryHandlerValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Product ID is required.")
				.Must(id => id != Guid.Empty).WithMessage("Product ID cannot be an empty GUID.");
		}
	}
}

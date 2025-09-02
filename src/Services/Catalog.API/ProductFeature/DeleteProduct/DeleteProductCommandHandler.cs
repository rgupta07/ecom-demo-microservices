
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.ProductFeature.DeleteProduct
{
	public record DeleteProductCommand(Guid Id): ICommand<DeleteProductResult>;
	public record DeleteProductResult();

	public class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
	{
		public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			var product = await session.LoadAsync<Product>(request.Id, cancellationToken);

			if (product == null)
			{
				throw new ProductNotFoundException(request.Id);
			}

			session.Delete(product);

			await session.SaveChangesAsync(cancellationToken);

			return new();
		}
	}

	public class DeleteProductCommandHandlerValidator : AbstractValidator<DeleteProductCommand>
	{
		public DeleteProductCommandHandlerValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Product ID is required.")
				.Must(id => id != Guid.Empty).WithMessage("Product ID cannot be an empty GUID.");
		}
	}
}

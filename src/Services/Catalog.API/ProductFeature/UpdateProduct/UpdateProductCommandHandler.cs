
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.ProductFeature.UpdateProduct
{
	public record UpdateProductCommand(
		Guid Id,
		string Name,
		string Description,
		IList<string> Category,
		string ImgFile,
		decimal Price) : ICommand<UpdateProductResult>;
	public record UpdateProductResult();

	public class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
	{
		public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var product = await session.LoadAsync<Product>(request.Id);

			if (product == null)
			{
				throw new ProductNotFoundException(request.Id);
			}

			product.Name = request.Name ?? product.Name;
			product.Description = request.Description ?? product.Description;
			product.Category = request.Category ?? product.Category;
			product.ImgFile = request.ImgFile ?? product.ImgFile;
			product.Price = request.Price > 0 ? request.Price : product.Price;

			session.Store(product);
			await session.SaveChangesAsync(cancellationToken);

			return new();
		}
	}

	public class UpdateProductCommandHandlerValidator : AbstractValidator<UpdateProductCommand>
	{
		public UpdateProductCommandHandlerValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Product ID is required.")
				.Must(id => id != Guid.Empty).WithMessage("Product ID cannot be an empty GUID.");
			RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
			RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
			RuleFor(x => x.ImgFile).NotEmpty().WithMessage("Image file is required.");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
		}
	}
}

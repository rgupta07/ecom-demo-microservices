using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.ProductFeature.CreateProduct
{
	public record CreateProductCommand(string Name, string Description, IList<string> Categories, string ImgFile, decimal Price)
		: ICommand<CreateProductResult>;
	public record CreateProductResult(Guid Id);

	public class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
	{
		public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
		{
			var product = new Product()
			{
				Id = Guid.NewGuid(),
				Name = command.Name,
				Description = command.Description,
				Category = command.Categories,
				ImgFile = command.ImgFile,
				Price = command.Price
			};

			//save in db : TO DO
			session.Store(product);
			await session.SaveChangesAsync(cancellationToken);

			//return guid
			return new CreateProductResult(product.Id);
		}
	}

	public class CreateProductCommandHandlerValidator : AbstractValidator<CreateProductCommand>
	{
		public CreateProductCommandHandlerValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
			RuleFor(x => x.Categories).NotEmpty().WithMessage("At least one category is required.");
			RuleFor(x => x.ImgFile).NotEmpty().WithMessage("Image file is required.");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
		}
	}
}

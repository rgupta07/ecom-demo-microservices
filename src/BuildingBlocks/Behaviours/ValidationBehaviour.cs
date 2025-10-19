using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviours
{
	public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators): IPipelineBehavior<TRequest, TResponse>
		where TRequest : ICommand<TResponse>
		where TResponse : notnull
	{
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var context = new ValidationContext<TRequest>(request);

			var results = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));

			var failures = results.Where(r => r.Errors.Count != 0).SelectMany(r => r.Errors).ToList();

			if (failures.Count != 0)
				throw new ValidationException(failures);

			return await next(cancellationToken);
		}
	}
}

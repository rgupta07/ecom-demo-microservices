using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviours
{
	public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
		where TRequest : notnull, IRequest<TResponse>
		where TResponse : notnull
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			logger.LogInformation("[START] Handling request of type {RequestType} with data: {@RequestData}", typeof(TRequest).Name, request);

			var timer = new Stopwatch();
			timer.Start();

			var result = await next(cancellationToken);

			timer.Stop();

			if (timer.ElapsedMilliseconds > 3000)
				logger.LogWarning("[PERFORMANCE] Request of type {RequestType} took {ElapsedMilliseconds} ms to complete", typeof(TRequest).Name, timer.ElapsedMilliseconds);

			logger.LogInformation("[END] Finished handling request of type {RequestType} in {ElapsedMilliseconds} ms", typeof(TRequest).Name, timer.ElapsedMilliseconds);

			return result;
		}
	}
}

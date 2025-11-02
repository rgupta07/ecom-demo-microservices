using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BuildingBlocks.Exceptions.Handler
{
	public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			logger.LogError($"Error Message: {exception.Message} at {DateTime.UtcNow}");

			(string Detail, string Title, int StatusCode) details = exception switch
			{
				InternalServerException =>
				(
					Detail: exception.Message,
					Title: exception.GetType().Name,
					StatusCode: StatusCodes.Status500InternalServerError
				),
				ValidationException => 
				(
					Detail: exception.Message,
					Title: exception.GetType().Name,
					StatusCode: StatusCodes.Status400BadRequest
				),
				NotFoundException => 
				(
					Detail: exception.Message,
					Title: exception.GetType().Name,
					StatusCode: StatusCodes.Status404NotFound
				),
				BadHttpRequestException => 
				(
					Detail: exception.Message,
					Title: exception.GetType().Name,
					StatusCode: StatusCodes.Status400BadRequest
				),
				_ => 
				(
					Detail: exception.Message,
					Title: exception.GetType().Name,
					StatusCode: StatusCodes.Status500InternalServerError
				)
			};

			var problemDetails = new ProblemDetails()
			{
				Detail = details.Detail,
				Title = details.Title,
				Status = details.StatusCode,
				Instance = httpContext.Request.Path
			};

			problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

			if(exception is ValidationException validationException)
			{
				problemDetails.Extensions.Add("Validation Errors", validationException.Errors);
			}

			httpContext.Response.StatusCode = details.StatusCode;
			httpContext.Response.ContentType = "application/problem+json";

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
			return true;
		}
	}
}

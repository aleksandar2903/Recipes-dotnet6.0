using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Recipes.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
	private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
	public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
	{
		_logger = logger;
	}
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
		try
		{
			await next(context);
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);

			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			ProblemDetails problem = new()
			{
				Status = (int)HttpStatusCode.InternalServerError,
				Title = "Internal Server Error",
				Type = "Internal Server Error",
				Detail = "An internal server error occur."
			};

			string json = JsonSerializer.Serialize(problem);

			context.Response.ContentType = "application/json";

			await context.Response.WriteAsync(json);
		}
    }
}

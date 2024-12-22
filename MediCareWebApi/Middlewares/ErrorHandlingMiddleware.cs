using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

public class ErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ErrorHandlingMiddleware> _logger;

	public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (InvalidOperationException ex)
		{
			await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict);
		}
		catch (ValidationException ex)
		{
			await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "An error occurred while processing the request.");
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode, string? customMessage = null)
	{
		_logger.LogError(ex, ex.Message);
		context.Response.StatusCode = (int)statusCode;
		context.Response.ContentType = "application/json";
		await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = customMessage ?? ex.Message }));
	}
}
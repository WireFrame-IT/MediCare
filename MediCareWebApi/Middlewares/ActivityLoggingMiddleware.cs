using MediCare.Services.Interfaces;

namespace MediCare.Middlewares
{
	public class ActivityLoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public ActivityLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, ILogService logService)
		{
			context.Request.EnableBuffering();
			var initialPosition = context.Request.Body.Position;

			try
			{
				await _next(context);
				await logService.LogAsync(context);
			}
			catch (Exception ex)
			{
				await logService.LogAsync(context, ex.Message);
				throw;
			}
			finally
			{
				context.Request.Body.Position = initialPosition;
			}
		}
	}

}

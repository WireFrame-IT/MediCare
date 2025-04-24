using System.Text;
using System.Text.RegularExpressions;
using MediCare.Models;
using MediCare.Services.Interfaces;
using MediCare.Utilities;

namespace MediCare.Services
{
	public partial class LogService : ILogService
	{
		[GeneratedRegex(@"""password""\s*:\s*""[^""]*""", RegexOptions.IgnoreCase, "pl-PL")]
		private static partial Regex MyRegex();

		private readonly MediCareDbContext _context;
		private readonly int _payloadCharacterLimit = 2000;

		public LogService(MediCareDbContext context)
		{
			_context = context;
		}

		public async Task LogAsync(HttpContext httpContext, string? errorMessage = null)
		{
			var log = new Log
			{
				Method = httpContext.Request.Method,
				Path = httpContext.Request.Path,
				StatusCode = httpContext.Response.StatusCode,
				CreatedAt = DateTime.UtcNow,
				IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
				UserAgent = httpContext.Request.Headers["User-Agent"].ToString(),
				QueryString = httpContext.Request.QueryString.HasValue ? httpContext.Request.QueryString.Value.TrimStart('?') : string.Empty,
				Payload = SanitizePayload(await GetRequestBody(httpContext)),
				Success = httpContext.Response.StatusCode < 400,
				UserId = httpContext.User.Identity?.IsAuthenticated == true ? httpContext.User.GetCurrentUserId() : null,
				ErrorMessage = errorMessage ?? string.Empty
			};

			_context.Logs.Add(log);
			await _context.SaveChangesAsync();
		}

		private async Task<string> GetRequestBody(HttpContext context)
		{
			if (context.Request.ContentLength == null || context.Request.Body == null || context.Request.ContentLength == 0)
				return string.Empty;

			context.Request.EnableBuffering();
			context.Request.Body.Position = 0;

			using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, 1024, true);
			var body = await reader.ReadToEndAsync();
			context.Request.Body.Position = 0;
			return body.Length > _payloadCharacterLimit ? body.Substring(0, _payloadCharacterLimit) : body;
		}

		private string SanitizePayload(string payload)
		{
			if (string.IsNullOrWhiteSpace(payload))
				return string.Empty;

			try
			{
				return MyRegex().Replace(payload, @"""password"":""******""");
			}
			catch
			{
				return payload;
			}
		}
	}
}

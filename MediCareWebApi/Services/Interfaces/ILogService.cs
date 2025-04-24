namespace MediCare.Services.Interfaces
{
	public interface ILogService
	{
		Task LogAsync(HttpContext httpContext, string? errorMessage = null);
	}
}

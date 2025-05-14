using System.IdentityModel.Tokens.Jwt;
using MediCare.Services.Interfaces;

public class TokenValidationMiddleware
{
	private readonly RequestDelegate _next;

	public TokenValidationMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
	{
		var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
		if (!string.IsNullOrEmpty(token))
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
			using var scope = serviceProvider.CreateScope();
			var accountsService = scope.ServiceProvider.GetRequiredService<IAccountsService>();

			if (jti != null && accountsService.IsTokenBlacklisted(jti))
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Token jest na czarnej liście.");
				return;
			}
		}
		await _next(context);
	}
}
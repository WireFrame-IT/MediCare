using System.Security.Claims;

namespace MediCare.Utilities
{
	public static class ClaimsPrincipalExtensions
	{
		public static int GetCurrentUserId(this ClaimsPrincipal user)
		{
			var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(userIdString, out var userId))
				throw new InvalidOperationException("User Id claim is not a valid integer.");
			return userId;
		}
	}
}

using MediCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MediCare.Controllers
{
	public class BaseController : Controller
	{

		protected readonly MediCareDbContext _context;
		protected readonly IConfiguration _configuration;

		protected BaseController(MediCareDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		protected int GetCurrentUserId()
		{
			var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(userIdString, out var userId))
				throw new InvalidOperationException("User Id claim is not a valid integer.");
			return userId;
		}

		protected async Task<User> GetCurrentUser()
		{
			var userId = GetCurrentUserId();
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
				throw new InvalidOperationException("User not found");
			return user;
		}
	}
}

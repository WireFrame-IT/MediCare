using MediCare.DTOs;
using MediCare.Enums;
using MediCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoMapper;
using MediCare.Services.Interfaces;

namespace MediCare.Controllers
{
	public class BaseController : Controller
	{

		protected readonly MediCareDbContext _context;
		protected readonly IConfiguration _configuration;
		protected readonly IMapper _mapper;
		protected readonly IAccountsService _accountsService;

		protected BaseController(MediCareDbContext context, IConfiguration configuration, IMapper mapper, IAccountsService accountsService)
		{
			_context = context;
			_configuration = configuration;
			_mapper = mapper;
			_accountsService = accountsService;
		}

		protected int GetCurrentUserId()
		{
			var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!int.TryParse(userIdString, out var userId))
				throw new InvalidOperationException("User Id claim is not a valid integer.");
			return userId;
		}

		protected async Task<User> GetCurrentUserAsync()
		{
			var userId = GetCurrentUserId();
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
				throw new InvalidOperationException("User not found");
			return user;
		}

		protected async Task<User> RegisterUserAsync(RegisterRequestDTO registerRequest, RoleType roleType)
		{
			var user = _mapper.Map<User>(registerRequest);
			user.Role = await _context.Roles.FirstAsync(x => x.RoleType == roleType);
			user.Password = _accountsService.HashPassword(registerRequest.Password, user.Salt = _accountsService.GenerateSalt());
			user.RefreshToken = _accountsService.GenerateSalt();
			user.RefreshTokenExpiration = DateTime.Now.AddHours(1);
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			return user;
		}
	}
}

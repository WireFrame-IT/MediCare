using MediCare.Enums;
using MediCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediCare.Services.Interfaces;
using MediCare.DTOs.Request;
using MediCare.Utilities;

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

		protected int GetCurrentUserId() => User.GetCurrentUserId();

		protected async Task<User> GetCurrentUserAsync()
		{
			var userId = GetCurrentUserId();
			var user = await _context.Users
				.Include(x => x.Role)
				.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
				throw new InvalidOperationException("User not found.");
			return user;
		}

		protected async Task<User> RegisterUserAsync(RegisterRequestDTO registerRequest, RoleType roleType)
		{
			var existingUser = await _context.Users
				.Where(x => x.Email == registerRequest.Email || x.Pesel == registerRequest.Pesel).FirstOrDefaultAsync();
			if (existingUser != null)
				throw new InvalidOperationException("User with the same email or PESEL already exists.");

			var user = _mapper.Map<User>(registerRequest);
			user.Role = await _context.Roles.FirstAsync(x => x.RoleType == roleType);
			user.Password = _accountsService.HashPassword(registerRequest.Password, user.Salt = _accountsService.GenerateSalt());
			user.RefreshToken = _accountsService.GenerateSalt();
			user.RefreshTokenExpiration = DateTime.Now.AddHours(1);
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			return user;
		}

		protected async Task<IActionResult?> CheckPermission(PermissionType type)
		{
			var user = await GetCurrentUserAsync();

			if (user.Role.RoleType == RoleType.Admin)
				return null;

			var rolePermission = await _context.RolePermissions.FirstOrDefaultAsync(x => x.Role.Id == user.Role.Id && x.Permission.PermissionType == type);
			return rolePermission == null ? Unauthorized("Missing required permission.") : null;
		}
	}
}

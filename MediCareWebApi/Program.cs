using System.Text;
using System.Text.Json.Serialization;
using MediCare.Mapping;
using MediCare.Middlewares;
using MediCare.Models;
using MediCare.ServiceModels;
using MediCare.Services;
using MediCare.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MediCareDbContext>(opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var secretKey = builder.Configuration.GetSection("JwtSettings:SecretKey").Value;
if (string.IsNullOrEmpty(secretKey))
{
	throw new InvalidOperationException("SecretKey is not set.");
}

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration.GetSection("JwtSettings:ValidIssuer").Value,
		ValidAudience = builder.Configuration.GetSection("JwtSettings:ValidAudience").Value,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
	};
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.WebHost.ConfigureKestrel(options =>
{
	options.ConfigureHttpsDefaults(httpsOptions =>
	{
		httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
	});
});

builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddAutoMapper(typeof(MediCareMappingProfile).Assembly);
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.Configure<AppointmentSettings>(builder.Configuration.GetSection("AppointmentSettings"));
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAllOrigins");
app.UseMiddleware<TransactionMiddleware>();
app.UseMiddleware<TokenValidationMiddleware>();
app.Run();
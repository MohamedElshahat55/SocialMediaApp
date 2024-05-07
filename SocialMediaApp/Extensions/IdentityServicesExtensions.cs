using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SocialMediaApp.Extensions
{
	public static class IdentityServicesExtensions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services , IConfiguration config) {
			// Configure JWT authentication
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

			return services;
		}
	}
}

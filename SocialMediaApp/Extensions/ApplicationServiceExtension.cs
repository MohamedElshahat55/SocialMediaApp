using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Services;

namespace SocialMediaApp.Extensions
{
	public static class ApplicationServiceExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			// Register Token Service
			services.AddScoped<ITokenService, TokenService>();

			//Add ConnectionString
			services.AddDbContext<DataContext>(opt =>
			{
				opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
			});

			services.AddCors();

			return services;
		}
	}
}

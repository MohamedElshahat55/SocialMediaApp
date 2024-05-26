using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Helpers;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Repositories;
using SocialMediaApp.Services;

namespace SocialMediaApp.Extensions
{
	public static class ApplicationServiceExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			// Register Token Service
			services.AddScoped<ITokenService, TokenService>();
			// Register IRepository
			services.AddScoped<IRepository,Repository>();
			// Register AutoMapper
			services.AddAutoMapper(typeof(AutomapperProfiles));

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

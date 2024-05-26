
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Data;
using SocialMediaApp.DataSeeding;
using SocialMediaApp.Extensions;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Middlewares;
using SocialMediaApp.Services;
using System.Text;

namespace SocialMediaApp
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// ApplicationSercicesExtensions
			builder.Services.AddApplicationServices(builder.Configuration);

			// IdentityServicesExtensions
			builder.Services.AddIdentityServices(builder.Configuration);

			var app = builder.Build();

            // Update Database
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            // Craete Object From ILogger
            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
               

            try
            {
                // Allow CLR Generete object Explicity for DataContext
                var _dbContext = services.GetRequiredService<DataContext>();
                await _dbContext.Database.MigrateAsync();
				await SeedUser.SeedAsync(_dbContext);
            }
            catch(Exception ex)
			{
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error has been occured during apply migration ");
            }



            // ADD CORS
            app.UseCors(builder=>builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
                // Add Exception Middleware
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication(); // Add this line to use authentication
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}

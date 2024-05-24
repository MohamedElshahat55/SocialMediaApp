
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Data;
using SocialMediaApp.Extensions;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Middlewares;
using SocialMediaApp.Services;
using System.Text;

namespace SocialMediaApp
{
	public class Program
	{
		public static void Main(string[] args)
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

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Hosting;
using SocialMediaApp.Errors;
using System;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace SocialMediaApp.Middlewares
{
    // Note) to use middleware ypo have register it in program file directely app.build
    //                app.UseMiddleware<ExceptionMiddleware>();

    //1. Class Definition and Dependencies:

    //public class ExceptionMiddleware =>  declares a public class named ExceptionMiddleware.

    //private readonly RequestDelegate _next;: =>  This injects a RequestDelegate dependency.Middleware components often use a RequestDelegate to reference the next middleware in the pipeline.

    //private readonly ILogger<ExceptionMiddleware> _logger;: =>  This injects an ILogger dependency specifically for the ExceptionMiddleware class. This allows logging exceptions.

    //private readonly IHostEnvironment _env;: This injects an IHostEnvironment dependency to distinguish between development and production environments.
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // await _next(context);: => This line delegates the request to the next middleware in the pipeline. If no exception occurs, the request processing continues normally.
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the Exception Details
                _logger.LogError(ex,ex.Message);
                //Sets the response content type to JSON to indicate a JSON response.
                context.Response.ContentType = "application/json";
                //Sets the response status code to 500 Internal Server Error.
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    // 1=> in Development
                    // If it's development, creates an ApiException object with the status code, message, and (optional) stack trace for debugging purposes.
                    ? new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
                    // 2 => in production
                    // if it's not development (production), creates an ApiException object with the status code and a generic message ("Internal Server Error") to avoid exposing sensitive information in production.
                    : new ApiException(context.Response.StatusCode,ex.Message,"Internal Server Error");

                // we will return this response as a json object
                //Creates an options object for JSON serialization, setting the PropertyNamingPolicy to camel case for consistency (optional).
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                //Writes the serialized JSON string to the response body.
                await context.Response.WriteAsync(json);
            }
        }
    }
}

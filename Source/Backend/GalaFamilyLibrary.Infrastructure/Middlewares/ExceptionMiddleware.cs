using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace GalaFamilyLibrary.Infrastructure.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                context.Response.ContentType = "application/problem+json";
                var problemDetails = new ProblemDetails()
                {
                    Title = $"An error occured: {e.Message}",
                    Detail = e.ToString(),
                    Status = 200
                };
                if (webHostEnvironment.IsDevelopment())
                {
                    problemDetails.Detail = e.ToString();
                }

                var stream = context.Response.Body;
                await JsonSerializer.SerializeAsync(stream, problemDetails);
            }
        }
    }

    public static class UseMiddleware
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
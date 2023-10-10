using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace GalaFamilyLibrary.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
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
                if (_webHostEnvironment.IsDevelopment())
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
using System.Net;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Consul;
using GalaFamilyLibrary.Infrastructure.Cors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

namespace GalaFamilyLibrary.Infrastructure.Middlewares
{
    public static class InfrastructureMiddleware
    {
        public static void UseInfrastructure(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            {
                app.UseSwagger();
                app.UseVersionedSwaggerUI();
            }

            app.UseMiddleware<NotFoundMiddleware>();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
                { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

            //app.UseConsul(app.Configuration);
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    var message = new MessageData(false, "An exception was thrown", 500);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(message));
                });
            });


            app.UseCorsService();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.UseSerilogLogging();

            app.Run();

            Log.CloseAndFlush();
        }

        public static void UseSerilogLogging(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "[{RemoteIpAddress}] [{RequestScheme}] [{RequestHost}] [{RequestMethod}] [{RequestPath}] responded [{StatusCode}] in [{Elapsed:0.0000}] ms";
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var value))
                    {
                        diagnosticContext.Set("RemoteIpAddress", value.ToString());
                    }
                    else
                    {
                        diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress?.MapToIPv4());
                    }
                };
            });
        }
    }
}
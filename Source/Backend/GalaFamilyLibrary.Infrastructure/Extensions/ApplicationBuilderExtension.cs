using GalaFamilyLibrary.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;

namespace GalaFamilyLibrary.Infrastructure.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseDefaultInfrastructure(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        {
            app.UseSwagger();
            app.UseDefaultSwaggerUI();
            app.MapGet("/", request =>
            {
                request.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });
        }

        app.UseDefaultExceptionHandler()
            .UseRouting()
            .UseCors(Options.CorsOptions.SectionName)
            .UseMiddleware<NotFoundMiddleware>()
            .UseAuthentication()
            .UseAuthorization()
            .UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        app.MapControllers();

        app.Run();

        return app;
    }

    public static IApplicationBuilder UseDefaultSwaggerUI(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        var apiVersionDescriptionProvider =
            app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
        return app;
    }

    public static IApplicationBuilder UseDefaultExceptionHandler(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status200OK;
                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();
                var message = new MessageData(false, "unexpected error");
                Log.Logger.Error(exceptionHandlerPathFeature?.Error.Message!);
                await context.Response.WriteAsync(message.Serialize());
            });
        });

        return app;
    }
}
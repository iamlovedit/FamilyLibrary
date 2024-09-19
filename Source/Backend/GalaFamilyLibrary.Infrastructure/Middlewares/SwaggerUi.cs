using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.Middlewares;

public static class SwaggerUI
{
    public static void UseVersionedSwaggerUI(this IApplicationBuilder app)
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
    }
}
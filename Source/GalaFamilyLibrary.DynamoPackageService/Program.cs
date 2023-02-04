using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
services.AddDbSetup();
services.AddSqlsugarSetup(builder.Configuration);
services.AddRepositorySetup();
services.AddCorsSetup();
services.AddApiVersionSetup();
services.AddVersionedApiExplorerSetup();
services.ConfigureOptions<ConfigureSwaggerOptions>();
services.AddScoped(typeof(IPackageService), typeof(PackageService));
services.AddControllers().AddProduceJsonSetup();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseCorsService();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
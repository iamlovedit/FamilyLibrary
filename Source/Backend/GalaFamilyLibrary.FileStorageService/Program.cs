using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.Extensions;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddDefaultSerilog(builder.Configuration);
// Add services to the container.
services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionsFilter));
});
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCorsSetup();
services.AddHttpClient();
services.AddFileSecurityOptionSetup(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapControllers();

app.Run();
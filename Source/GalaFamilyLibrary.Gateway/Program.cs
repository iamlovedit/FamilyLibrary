using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();
builder.Configuration.AddJsonFile("ocelot.json", false, true);

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.UseOcelot().Wait();
app.Run();
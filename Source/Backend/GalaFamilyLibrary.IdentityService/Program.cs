using FluentValidation;
using GalaFamilyLibrary.DataTransferObject.Identity;
using GalaFamilyLibrary.Infrastructure.Extensions;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Service.Identity;
using GalaFamilyLibrary.Service.Validators;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddScoped<IUserService, UserService>();
services.AddScoped<IValidator<UserCreationDTO>, UserCreationValidator>();
builder.AddDefaultInfrastructure<long>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseInitSeed(dbSeed =>
{
    dbSeed.GenerateTablesByClass<User>();
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }

    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Users");
    dbSeed.GenerateSeedAsync<User>(file);

    file = string.Format(seedFolder, "Roles");
    dbSeed.GenerateSeedAsync<Role>(file);

    file = string.Format(seedFolder, "UserRoles");
    dbSeed.GenerateSeedAsync<UserRole>(file);

    file = string.Format(seedFolder, "FamilyCollections");
    dbSeed.GenerateSeedAsync<FamilyCollections>(file);

    // file = string.Format(seedFolder, "FamilyStars");
    // dbSeed.InitSeed<FamilyStar>(file);
});
app.UseInfrastructure();
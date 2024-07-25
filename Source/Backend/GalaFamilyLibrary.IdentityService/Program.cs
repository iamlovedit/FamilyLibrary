using FluentValidation;
using GalaFamilyLibrary.DataTransferObject.Identity;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Service.Identity;
using GalaFamilyLibrary.Service.Validators;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
services.AddFileSecurityOptionSetup(builder.Configuration);
services.AddFileStorageClientSetup(builder.Configuration);

services.AddScoped<IUserService, UserService>();
services.AddScoped<IValidator<UserCreationDTO>, UserCreationValidator>();
builder.AddInfrastructureSetup();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTablesByClass<User>();
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }

    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Users");
    dbSeed.InitSeed<User>(file);

    file = string.Format(seedFolder, "Roles");
    dbSeed.InitSeed<Role>(file);

    file = string.Format(seedFolder, "UserRoles");
    dbSeed.InitSeed<UserRole>(file);

    file = string.Format(seedFolder, "FamilyCollections");
    dbSeed.InitSeed<FamilyCollection>(file);

    // file = string.Format(seedFolder, "FamilyStars");
    // dbSeed.InitSeed<FamilyStar>(file);
});
app.UseInfrastructure();
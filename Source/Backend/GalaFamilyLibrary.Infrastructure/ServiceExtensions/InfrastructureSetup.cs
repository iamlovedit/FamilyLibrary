using System.IdentityModel.Tokens.Jwt;
using GalaFamilyLibrary.Infrastructure.AutoMapper;
using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.Filters;
using GalaFamilyLibrary.Infrastructure.Security;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using GalaFamilyLibrary.Repository;
using GalaFamilyLibrary.Repository.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class InfrastructureSetup
    {
        public static void AddInfrastructureSetup(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddSingleton<GalaTokenHandler>();
            services.AddSingleton<IAESEncryptionService, AESEncryptionService>();
            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerOptionsPostConfigureOptions>();
            services.AddSingleton<ITokenBuilder, TokenBuilder>();
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            //services.AddDataProtectionSetup();

            services.AddAutoMapperSetup();

            services.AddDatabaseSetup();

            services.AddRedisCacheSetup(configuration);

            builder.AddSerilogSetup();

            services.AddJwtAuthenticationSetup(configuration);

            services.AddAuthorizationSetup(configuration);

            //sql sugar
            services.AddSqlsugarSetup(configuration, builder.Environment);
            //route
            services.AddRoutingSetup();
            //cors 
            services.AddCorsSetup();
            //api version
            services.AddApiVersionSetup();

            services.AddControllers(options => { options.Filters.Add(typeof(GlobalExceptionsFilter)); })
                .AddProduceJsonSetup();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();
        }
    }
}
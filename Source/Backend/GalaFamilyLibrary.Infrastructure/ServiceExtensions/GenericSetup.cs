using GalaFamilyLibrary.Infrastructure.AutoMapper;
using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.Filters;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Security;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class GenericSetup
    {
        public static void AddGenericSetup(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddSingleton<IAESEncryptionService, AESEncryptionService>();
            services.AddSingleton<GalaTokenValidator>();
            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerOptionsPostConfigureOptions>();
            services.AddSingleton<ITokenBuilder, TokenBuilder>();

            services.AddDataProtectionSetup();

            services.AddAutoMapperSetup();

            services.AddDbSetup();


            //services.AddConsulConfigSetup(configuration);

            services.AddRedisCacheSetup(configuration);

            services.AddSeqSetup(configuration);

            services.AddAuthorizationSetup(configuration);

            services.AddJwtAuthentication(configuration);

            //sqlsugar
            services.AddSqlsugarSetup(configuration, builder.Environment);
            //route
            services.AddRoutingSetup();
            //repository
            services.AddRepositorySetup();
            //cors 
            services.AddCorsSetup();
            //api version
            services.AddApiVersionSetup();

            services.AddControllers(options => { options.Filters.Add(typeof(GlobalExceptionsFilter)); })
                .AddProduceJsonSetup();

            services.AddVersionedApiExplorerSetup();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();
        }
    }
}
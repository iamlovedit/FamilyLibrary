using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.Filters;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using GalaFamilyLibrary.Infrastructure.AutoMapper;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class GenericSetup
    {
        public static void AddGenericSetup(this WebApplicationBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddAESEncryptionSetup(configuration);

            services.AddDataProtectionSetup();

            services.AddAutoMapperSetup();

            services.AddDbSetup();

            builder.AddTraceOutputSetup();

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
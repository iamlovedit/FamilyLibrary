using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbSetup();

            builder.AddTraceOutputSetup();

            //services.AddConsulConfigSetup(configuration);

            services.AddRedisCacheSetup(configuration);

            services.AddSeqSetup(configuration);

            services.AddAuthorizationSetup(configuration);

            services.AddJwtAuthentication(configuration);

            //sqlsugar
            services.AddSqlsugarSetup(configuration);
            //route
            services.AddRoutingSetup();
            //repository
            services.AddRepositorySetup();
            //cors 
            services.AddCorsSetup();
            //api version
            services.AddApiVersionSetup();

            services.AddControllers().AddProduceJsonSetup();

            services.AddVersionedApiExplorerSetup();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();
        }
    }
}
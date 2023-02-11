using System.Reflection;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Consul;
using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class GenericSetup
    {
        public static void AddGenericSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbSetup();

            services.AddConsulConfigSetup(configuration);

            services.AddRedisCacheSetup(configuration);

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

            services.AddAuthorizationSetup(configuration);
        }
    }
}
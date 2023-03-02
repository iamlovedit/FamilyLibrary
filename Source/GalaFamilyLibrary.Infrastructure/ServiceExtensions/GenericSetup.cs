using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });
            services.Configure<AESEncryptionOption>(configuration.GetSection(nameof(AESEncryptionOption)));

            services.AddSingleton<DataProtectionHelper>();

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
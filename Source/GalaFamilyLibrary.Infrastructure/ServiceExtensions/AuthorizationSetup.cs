using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var key = configuration.GetSection("Audience")["Key"];
            var keyByteArray = Encoding.ASCII.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var issuer = configuration.GetSection("Audience")["Issuer"];
            var audience = configuration.GetSection("Audience")["Audience"];

            services.AddSingleton(new PermissionRequirement(ClaimTypes.Role, audience, issuer, TimeSpan.FromSeconds(60 * 60), signingCredentials));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Consumer", policy => policy.RequireRole("Consumer").Build());
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator").Build());
            });
            //custom  authorization
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // services.AddScoped<IAuthorizationHandler, PermissionHandler>();
        }
    }
}

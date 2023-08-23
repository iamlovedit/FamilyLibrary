using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;
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

            var key = configuration["AUDIENCE_KEY"];
            var keyByteArray = Encoding.ASCII.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var issuer = configuration["AUDIENCE_ISSUER"];
            var audience = configuration["AUDIENCE_AUDIENCE"];
            var expiration = configuration["AUDIENCE_EXPIRATION"];

            services.AddSingleton(new PermissionRequirement(ClaimTypes.Role, issuer, audience,
                TimeSpan.FromSeconds(expiration.ObjToInt()), signingCredentials));
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PermissionConstants.POLICYNAME,
                    policy => policy.RequireRole(PermissionConstants.ROLE_SUPERADMINISTRATOR,
                        PermissionConstants.ROLE_ADMINISTRATOR, PermissionConstants.ROLE_CONSUMER).Build());
            });
        }
    }
}
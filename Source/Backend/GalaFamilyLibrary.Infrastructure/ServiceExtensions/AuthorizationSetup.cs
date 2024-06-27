using GalaFamilyLibrary.Infrastructure.Common;
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
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(configuration);

            var audienceSection = configuration.GetSection("Audience");
            var keyByteArray = Encoding.ASCII.GetBytes(configuration["AUDIENCE_KEY"]!);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var issuer = audienceSection["Issuer"];
            var audience = audienceSection["Audience"];
            var expiration = audienceSection["Expiration"];

            services.AddSingleton(new PermissionRequirement(ClaimTypes.Role, issuer, audience,
                TimeSpan.FromSeconds(expiration.ObjToInt()), signingCredentials));
            services.AddAuthorizationBuilder()
                .AddPolicy(PermissionConstants.POLICYNAME, policy => policy.RequireRole(PermissionConstants.ROLE_SUPERADMINISTRATOR,
                        PermissionConstants.ROLE_ADMINISTRATOR, PermissionConstants.ROLE_CONSUMER).Build());
        }
    }
}
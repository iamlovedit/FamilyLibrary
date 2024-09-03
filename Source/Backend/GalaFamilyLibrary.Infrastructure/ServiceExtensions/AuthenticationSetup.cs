using System.Text;
using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using GalaFamilyLibrary.Infrastructure.OAuth.Gitee;
using GalaFamilyLibrary.Infrastructure.Security;
using Microsoft.Extensions.Primitives;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class AuthenticationSetup
{
    public static void AddAuthenticationSetup(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection("Audience");
        var buffer = Encoding.UTF8.GetBytes(configuration["AUDIENCE_KEY"]!);
        var key = new SymmetricSecurityKey(buffer);
        var issuer = section["Issuer"];
        var audience = section["Audience"];
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidIssuer = issuer,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(15),
            RequireExpirationTime = true,
            RoleClaimType = ClaimTypes.Role
        };

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = nameof(GalaAuthenticationHandler);
            options.DefaultForbidScheme = nameof(GalaAuthenticationHandler);
        }).AddScheme<AuthenticationSchemeOptions, GalaAuthenticationHandler>(nameof(GalaAuthenticationHandler),
            options => { }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
            options.Events = new JwtBearerEvents()
            {
                OnChallenge = challengeContext =>
                {
                    challengeContext.Response.Headers.Append(
                        new KeyValuePair<string, StringValues>("token-error", challengeContext.ErrorDescription));
                    return Task.CompletedTask;
                },
            };
        })
        .AddGiteeAuthentication();
    }
}
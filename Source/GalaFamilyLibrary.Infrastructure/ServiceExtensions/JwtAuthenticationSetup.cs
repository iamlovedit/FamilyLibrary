using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class JwtAuthenticationSetup
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var section = configuration.GetSection("Audience");
        var buffer = Encoding.UTF8.GetBytes(section["Key"]);
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
            ClockSkew = TimeSpan.FromSeconds(30),
            RequireExpirationTime = true,
        };

        void ConfigureJwtBearer(JwtBearerOptions options)
        {
            options.TokenValidationParameters = tokenValidationParameters;
            options.Events = new JwtBearerEvents()
            {
                OnChallenge = challengeContext =>
                {
                    challengeContext.Response.Headers.Add("Token-Error", challengeContext.ErrorDescription);
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = failedContext =>
                {
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var token = failedContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");

                    if (failedContext.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        failedContext.Response.Headers.Add("token-expired", "true");
                    }

                    if (string.IsNullOrEmpty(token) || !jwtHandler.CanReadToken(token))
                    {
                        failedContext.Response.Headers.Add("token-error", "can not get token");
                        return Task.CompletedTask;
                    }

                    var jwtToken = jwtHandler.ReadJwtToken(token);
                    if (jwtToken.Issuer != issuer)
                    {
                        failedContext.Response.Headers.Add("token-error-issuer", "issuer is wrong!");
                    }

                    if (jwtToken.Audiences.FirstOrDefault() != audience)
                    {
                        failedContext.Response.Headers.Add("token-error-audience", "audience is wrong!");
                    }

                    return Task.CompletedTask;
                }
            };
        }

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ApiResponseHandler);
                options.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
            .AddJwtBearer(ConfigureJwtBearer)
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), options => { });
    }
}
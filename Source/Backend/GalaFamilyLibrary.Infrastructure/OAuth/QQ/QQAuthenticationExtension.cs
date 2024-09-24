using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;

namespace GalaFamilyLibrary.Infrastructure.OAuth.QQ
{
    public static class QQAuthenticationExtension
    {
        public const string AuthenticationSchemeName = "QQ";

        public static AuthenticationBuilder AddQQAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddQQAuthentication(AuthenticationSchemeName, options => { });
        }

        public static AuthenticationBuilder AddQQAuthentication(this AuthenticationBuilder builder,
            Action<QQAuthenticationOptions> configureOptions)
        {
            return builder.AddQQAuthentication(AuthenticationSchemeName, configureOptions);
        }

        public static AuthenticationBuilder AddQQAuthentication(
            this AuthenticationBuilder builder,
            string scheme,
            Action<QQAuthenticationOptions> configureOptions)
        {
            return builder.AddQQAuthentication(scheme, AuthenticationSchemeName, configureOptions);
        }

        public static AuthenticationBuilder AddQQAuthentication(this AuthenticationBuilder builder, string scheme,
            string displayName,
            Action<QQAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<QQAuthenticationOptions, QQAuthenticationHandler>(scheme, displayName,
                configureOptions);
        }
    }
}
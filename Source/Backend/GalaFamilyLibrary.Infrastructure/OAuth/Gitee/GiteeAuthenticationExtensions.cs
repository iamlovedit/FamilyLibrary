using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;

namespace GalaFamilyLibrary.Infrastructure.OAuth.Gitee
{
    public static class GiteeAuthenticationExtensions
    {
        public static string AuthenticationSchemeName = "Gitee";

        public static AuthenticationBuilder AddGiteeAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddGiteeAuthentication(AuthenticationSchemeName, options => { });
        }

        public static AuthenticationBuilder AddGiteeAuthentication(this AuthenticationBuilder builder,
            Action<GiteeAuthenticationOptions> configuration)
        {
            return builder.AddGiteeAuthentication(AuthenticationSchemeName, configuration);
        }

        public static AuthenticationBuilder AddGiteeAuthentication(this AuthenticationBuilder builder, string scheme,
            Action<GiteeAuthenticationOptions> configureOptions)
        {
            return builder.AddGiteeAuthentication(scheme, AuthenticationSchemeName, configureOptions);
        }

        public static AuthenticationBuilder AddGiteeAuthentication(this AuthenticationBuilder builder, string scheme,
            string caption,
            Action<GiteeAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<GiteeAuthenticationOptions, GiteeAuthenticationHandler>(scheme, caption,
                configureOptions);
        }
    }
}
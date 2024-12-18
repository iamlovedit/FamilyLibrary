﻿using System.Diagnostics.CodeAnalysis;

namespace GalaFamilyLibrary.Infrastructure.OAuth.Gitee
{
    public static class GiteeAuthenticationExtensions
    {
        public const string AuthenticationSchemeName = "Gitee";

        public static AuthenticationBuilder AddGiteeAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddGiteeAuthentication(AuthenticationSchemeName, options => { });
        }

        public static AuthenticationBuilder AddGiteeAuthentication(this AuthenticationBuilder builder,
            Action<GiteeAuthenticationOptions> configureOptions)
        {
            return builder.AddGiteeAuthentication(AuthenticationSchemeName, configureOptions);
        }

        public static AuthenticationBuilder AddGiteeAuthentication(this AuthenticationBuilder builder, string scheme,
            Action<GiteeAuthenticationOptions> configureOptions)
        {
            return builder.AddGiteeAuthentication(scheme, AuthenticationSchemeName, configureOptions);
        }

        public static AuthenticationBuilder AddGiteeAuthentication(this AuthenticationBuilder builder, string scheme,
            string displayName,
            Action<GiteeAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<GiteeAuthenticationOptions, GiteeAuthenticationHandler>(scheme, displayName,
                configureOptions);
        }
    }
}
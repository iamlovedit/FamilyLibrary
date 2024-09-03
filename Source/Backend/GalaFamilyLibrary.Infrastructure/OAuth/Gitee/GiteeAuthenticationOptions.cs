using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace GalaFamilyLibrary.Infrastructure.OAuth.Gitee
{
    public class GiteeAuthenticationOptions : AuthenticationOAuthOptions
    {
        public string UserEmailsEndpoint { get; } = "https://gitee.com/api/v5/emails";

        public string Url { get; } = "urn:gitee:url";

        public string AvatarUrl { get; } = "urn:gitee:avatarUrl";

        public GiteeAuthenticationOptions()
        {
            ClaimsIssuer = GiteeAuthenticationExtensions.AuthenticationSchemeName;
            CallbackPath = "/signin-gitee";
            AuthorizationEndpoint = "https://gitee.com/oauth/authorize";
            TokenEndpoint = "https://gitee.com/oauth/token";
            UserInformationEndpoint = "https://gitee.com/api/v5/user";

            Scope.Add("user_info");
            Scope.Add("emails");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Actor, "login");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(Url, "url");
        }
    }
}
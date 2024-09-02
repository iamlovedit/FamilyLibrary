using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace GalaFamilyLibrary.Infrastructure.OAuth.Gitee
{
    public class GiteeAuthenticationOptions : AuthenticationOAuthOptions
    {
        public static string SchemaName = "Gitee";
        public string UserEmailsEndpoint { get; set; }

        public string Url { get; set; } = "urn:gitee:url";

        public string AvatarUrl { get; set; } = "urn:gitee:avatarUrl";

        public GiteeAuthenticationOptions()
        {
            ClaimsIssuer = SchemaName;
            CallbackPath = "/signin-gitee";
            AuthorizationEndpoint = "https://gitee.com/oauth/authorize";
            TokenEndpoint = "https://gitee.com/oauth/token";
            UserInformationEndpoint = "https://gitee.com/api/v5/user";
            UserEmailsEndpoint = "https://gitee.com/api/v5/emails";

            Scope.Add("user_info");
            Scope.Add("emails");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(Url, "url");
        }
    }
}
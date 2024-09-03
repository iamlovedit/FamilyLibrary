using Microsoft.AspNetCore.Authentication.OAuth;

namespace GalaFamilyLibrary.Infrastructure.OAuth
{
    public class AuthenticationOAuthOptions : OAuthOptions
    {
        public virtual string RedirectUri { get; set; }

        public virtual string OpenId { get; set; } = "urn:openid";

        public virtual string AccessToken { get; set; } = "urn:access_token";

        public virtual string Name { get; set; } = "urn:name";
    }
}
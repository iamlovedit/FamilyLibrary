using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace GalaFamilyLibrary.Infrastructure.OAuth.QQ
{
    public class QQAuthenticationOptions : AuthenticationOAuthOptions
    {
        public string AvatarFullUrl { get; } = "urn:qq:avatar_full";

        public string AvatarUrl { get; } = "urn:qq:avatar";

        public string PictureFullUrl { get; } = "urn:qq:picture_full";

        public string PictureMediumUrl { get; } = "urn:qq:picture_medium";

        public string PictureUrl { get; } = "urn:qq:picture";

        public string UnionId { get; } = "urn:qq:unionid";

        public string UserIdentificationEndpoint { get; } = "https://graph.qq.com/oauth2.0/me";

        public override string RedirectUri { get; set; }

        public QQAuthenticationOptions()
        {
            ClaimsIssuer = QQAuthenticationExtension.AuthenticationSchemeName;
            CallbackPath = "/signin-qq";

            AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";
            TokenEndpoint = "https://graph.qq.com/oauth2.0/token";
            UserInformationEndpoint = "https://graph.qq.com/user/get_user_info";

            Scope.Add("get_user_info");

            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(PictureUrl, "figureurl");
            ClaimActions.MapJsonKey(PictureMediumUrl, "figureurl_1");
            ClaimActions.MapJsonKey(PictureFullUrl, "figureurl_2");
            ClaimActions.MapJsonKey(AvatarUrl, "figureurl_qq_1");
            ClaimActions.MapJsonKey(AvatarFullUrl, "figureurl_qq_2");
        }
    }
}
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GalaFamilyLibrary.Infrastructure.OAuth.QQ
{
    public class QQAuthenticationHandler(
        IOptionsMonitor<QQAuthenticationOptions> options,
        ILoggerFactory logger,
        IHttpClientFactory httpClientFactory,
        UrlEncoder encoder) :
        OAuthenticationHandlerBase<QQAuthenticationOptions>(options, logger, httpClientFactory, encoder)
    {
        protected override string AuthenticationSchemeName { get; set; } =
            QQAuthenticationExtension.AuthenticationScheme;

        protected override async Task<IList<Claim>> GenerateClaimsByCode(string code)
        {
            var tokenQueryPairs = new List<KeyValuePair<string, string?>>
            {
                new("grant_type", "authorization_code"),
                new("client_id", Options.ClientId),
                new("client_secret", Options.ClientSecret),
                new("redirect_uri", Options.RedirectUri),
                new("fmt", "json"),
                new("need_openid", "1"),
                new("code", code)
            };

            var qqToken = await SeedHttpMessageAsync<QQToken>(Options.TokenEndpoint, tokenQueryPairs);

            var userInfoQueryPairs = new List<KeyValuePair<string, string?>>
            {
                new("access_token", qqToken.AccessToken),
                new("oauth_consumer_key", Options.ClientId),
                new("openid", qqToken.OpenId)
            };

            var userInfo = await SeedHttpMessageAsync<QQUserInfo>(Options.UserInformationEndpoint, userInfoQueryPairs);

            var claims = new List<Claim>()
            {
                new(Options.AvatarFullUrl, userInfo.figureurl_qq_2),
                new(Options.AvatarUrl, userInfo.figureurl_qq_1),
                new(Options.PictureFullUrl, userInfo.figureurl_2),
                new(Options.PictureMediumUrl, userInfo.figureurl_qq_1),
                new(Options.PictureUrl, userInfo.figureurl),

                new(Options.OpenId, qqToken.OpenId),
                new(Options.Name, userInfo.nickname),
                new(Options.AccessToken, qqToken.AccessToken),
            };

            return claims;
        }
    }
}
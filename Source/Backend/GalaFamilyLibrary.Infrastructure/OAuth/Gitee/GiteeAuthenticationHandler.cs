using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GalaFamilyLibrary.Infrastructure.OAuth.Gitee
{
    public class GiteeAuthenticationHandler(
        IOptionsMonitor<GiteeAuthenticationOptions> options,
        ILoggerFactory logger,
        IHttpClientFactory httpClientFactory,
        UrlEncoder encoder)
        : OAuthenticationHandlerBase<GiteeAuthenticationOptions>(options, logger, httpClientFactory, encoder)
    {
        protected override string AuthenticationSchemeName { get; set; } =
            GiteeAuthenticationExtensions.AuthenticationSchemeName;

        protected override async Task<IList<Claim>> GenerateClaimsByCode(string code)
        {
            var tokenQueryPairs = new List<KeyValuePair<string, string?>>()
            {
                new("grant_type", "authorization_code"),
                new("client_id", Options.ClientId),
                new("client_secret", Options.ClientSecret),
                new("redirect_uri", Options.RedirectUri),
                new("code", code)
            };
            var giteeToken = await SeedHttpMessageAsync<GiteeToken>(Options.TokenEndpoint, tokenQueryPairs,HttpMethod.Post);

            var userInfoQueryPairs = new List<KeyValuePair<string, string?>>()
            {
                new("access_token", giteeToken.AccessToken),
            };

            var userInfo =
                await SeedHttpMessageAsync<GiteeUserInfo>(Options.UserInformationEndpoint, userInfoQueryPairs);
            var claims = new List<Claim>()
            {
                new(Options.AvatarUrl, userInfo.AvatarUrl),
                new(Options.Url, userInfo.Url),

                new(Options.OpenId, userInfo.Id.ToString()),
                new(Options.Name, userInfo.Name),
                new(Options.AccessToken, giteeToken.AccessToken)
            };
            return claims;
        }
    }
}
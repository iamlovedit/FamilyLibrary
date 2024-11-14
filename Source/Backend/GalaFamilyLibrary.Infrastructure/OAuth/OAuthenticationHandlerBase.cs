using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace GalaFamilyLibrary.Infrastructure.OAuth
{
    public abstract class OAuthenticationHandlerBase<TOptions>(
        IOptionsMonitor<TOptions> options,
        ILoggerFactory logger,
        IHttpClientFactory httpClientFactory,
        UrlEncoder encoder)
        : AuthenticationHandler<TOptions>(options, logger, encoder)
        where TOptions : AuthenticationSchemeOptions, new()
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        protected abstract string AuthenticationSchemeName { get; set; }
        protected abstract Task<IList<Claim>> GenerateClaimsByCode(string code);

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Query.ContainsKey("code"))
            {
                return AuthenticateResult.Fail("回调未包含code参数");
            }

            var code = Context.Request.Query["code"].ToString();
            try
            {
                var claims = await GenerateClaimsByCode(code);
                var claimsIdentity = new ClaimsIdentity(claims.ToArray(), AuthenticationSchemeName);
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), AuthenticationSchemeName);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail(e?.Message ?? "未知错误");
            }
        }

        protected async Task<T?> SeedHttpMessageAsync<T>(string url, IEnumerable<KeyValuePair<string, string?>> query,
            HttpMethod? httpMethod = null)
        {
            httpMethod = httpMethod ?? HttpMethod.Get;

            var queryUrl = QueryHelpers.AddQueryString(url, query);

            var response = default(HttpResponseMessage);
            if (httpMethod == HttpMethod.Get)
            {
                response = await _httpClient.GetAsync(queryUrl);
            }
            else if (httpMethod == HttpMethod.Post)
            {
                response = await _httpClient.PostAsync(queryUrl, null);
            }

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"授权服务器请求错误,请求地址:{queryUrl},错误信息：{content}");
            }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace GalaFamilyLibrary.Gateway.Extensions
{
    public class GatewayAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IRedisBasketRepository _redis;
        public GatewayAuthenticationHandler(IRedisBasketRepository redis, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _redis = redis;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //获取header的token 然后验证
            throw new NotImplementedException();
        }
    }
}

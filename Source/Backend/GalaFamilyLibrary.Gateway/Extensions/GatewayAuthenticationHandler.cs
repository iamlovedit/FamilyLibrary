using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Gateway.Extensions
{
    public class GatewayAuthenticationHandler(
        IRedisBasketRepository redis,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
    {
        private readonly IRedisBasketRepository _redis = redis;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //获取header的token 然后验证
            var tokenString = Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
            
            throw new NotImplementedException();
        }
    }
}
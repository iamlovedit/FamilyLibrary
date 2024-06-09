using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class JwtBearerOptionsPostConfigureOptions(GalaTokenValidator galaTokenValidator)
        : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            options.SecurityTokenValidators.Clear();
            options.SecurityTokenValidators.Add(galaTokenValidator);
        }
    }
}

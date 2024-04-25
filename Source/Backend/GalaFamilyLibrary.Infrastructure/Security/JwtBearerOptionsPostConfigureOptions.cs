using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class JwtBearerOptionsPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly GalaTokenValidator _galaTokenValidator;

        public JwtBearerOptionsPostConfigureOptions(GalaTokenValidator galaTokenValidator)
        {
            _galaTokenValidator = galaTokenValidator;
        }

        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            options.SecurityTokenValidators.Clear();
            options.SecurityTokenValidators.Add(_galaTokenValidator);
        }
    }
}

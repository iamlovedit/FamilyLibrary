namespace GalaFamilyLibrary.Infrastructure.Security;

public class JwtBearerOptionsPostConfigureOptions(
    DefaultTokenHandler tokenHandler)
    : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenHandlers.Clear();
        options.TokenHandlers.Add(tokenHandler);
    }
}
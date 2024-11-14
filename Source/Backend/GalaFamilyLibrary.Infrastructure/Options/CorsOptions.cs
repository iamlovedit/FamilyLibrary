namespace GalaFamilyLibrary.Infrastructure.Options;

public class CorsOptions : OptionsBase
{
    public const string SectionName = "Cors";

    public bool AllowAnyMethod { get; set; }

    public string[] Methods { get; set; }

    public bool AllowAnyHeader { get; set; }

    public string[] Headers { get; set; }

    public bool AllowAnyOrigin { get; set; }

    public string[] Origins { get; set; }
}
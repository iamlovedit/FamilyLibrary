namespace GalaFamilyLibrary.Infrastructure.Options;

/// <summary>
/// api版本配置
/// </summary>
public class VersionOptions : OptionsBase
{
    public const string SectionName = "Version";
    
    public string HeaderName { get; set; }

    public string ParameterName { get; set; }

    public string SwaggerTitle { get; set; }
}
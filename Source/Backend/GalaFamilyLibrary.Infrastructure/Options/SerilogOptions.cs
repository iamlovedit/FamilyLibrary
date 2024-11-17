namespace GalaFamilyLibrary.Infrastructure.Options;

public class SerilogOptions : OptionsBase
{
    public const string Name = "Serilog";

    public bool WriteFile { get; set; }

    public SeqOptions? SeqOptions { get; set; }
}

public class SeqOptions : OptionsBase
{
    public const string SeqUrl= "SEQ_URL";

    public const string SeqApiKey = "SEQ_APIKEY";
    
    public string Url { get; set; }

    public string Secret { get; set; }
}
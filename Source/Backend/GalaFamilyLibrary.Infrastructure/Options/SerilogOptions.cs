namespace GalaFamilyLibrary.Infrastructure.Options;

public class SerilogOptions : OptionsBase
{
    public const string Name = "Serilog";

    public bool WriteFile { get; set; }

    public SeqOptions? SeqOptions { get; set; }
}

public class SeqOptions : OptionsBase
{
    public string Address { get; set; }

    public string Secret { get; set; }
}
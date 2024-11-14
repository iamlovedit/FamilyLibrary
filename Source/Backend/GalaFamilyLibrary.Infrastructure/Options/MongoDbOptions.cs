namespace GalaFamilyLibrary.Infrastructure.Options;

public class MongoDbOptions : OptionsBase
{
    public const string SectionName = "MongoDb";

    public const string MongoPassword = "MONGO_PASSWORD";

    public const string MongoUser = "MONGO_USER";

    public const string MongoDatabase = "MONGO_DATABASE";

    public const string MongoHost = "MONGO_HOST";

    public const string MongoPort = "MONGO_PORT";

    public string User { get; set; }
    public string Password { get; set; }
    public string Port { get; set; }
    public string Host { get; set; }
    public string? Database { get; set; }
}
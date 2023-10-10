using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Infrastructure.FileStorage
{
    public class FileSecurityOption
    {
        [JsonConstructor]
        public FileSecurityOption() { }
        public FileSecurityOption(IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(FileSecurityOption));

            Timespan = (uint)section[nameof(Timespan)].ObjToInt();
            AccessKey = section[nameof(AccessKey)];
        }

        public DateTime Expiration { get; set; }

        public uint Timespan { get; set; }

        public string AccessKey { get; set; }

        public string Filename { get; set; }

        public string GetAccessToken(string filename)
        {
            var token = new
            {
                expiration = DateTime.Now.AddSeconds(Timespan),
                accessKey = AccessKey,
                filename = filename
            };
            return JsonConvert.SerializeObject(token);
        }
    }
}

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Infrastructure.FileStorage
{
    public class FileSecurityOption
    {
        public FileSecurityOption(IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(FileSecurityOption));

            Expiration = section[nameof(Expiration)].ObjToDate();
            Timespan = (uint)section[nameof(Timespan)].ObjToInt();
            AccessKey = section[nameof(AccessKey)];
        }

        public DateTime Expiration { get; }

        public uint Timespan { get; }

        public string AccessKey { get; }

        public string GetAccessToken()
        {
            var token = new
            {
                expiration = DateTime.Now.AddSeconds(Timespan),
                accessKey = AccessKey,
            };
            return JsonConvert.SerializeObject(token);
        }
        public static FileSecurityOption GetOption(string accessToken)
        {
            try
            {
                return JsonConvert.DeserializeObject<FileSecurityOption>(accessToken);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}

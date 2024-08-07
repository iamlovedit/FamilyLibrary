using Newtonsoft.Json;

namespace GalaFamilyLibrary.Infrastructure.OAuth.Gitee
{
    public class GiteeUserInfo()
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("login")]
        public string Login { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
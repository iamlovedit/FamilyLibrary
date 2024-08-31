using Newtonsoft.Json;

namespace GalaFamilyLibrary.Infrastructure.OAuth.QQ
{
    public class QQToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
                
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        
        [JsonProperty("openid")]
        public string OpenId { get; set; }
    }
}
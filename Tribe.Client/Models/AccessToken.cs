using Newtonsoft.Json;

namespace Tribe.Client.Models
{
    public class AccessToken
    {
        [JsonProperty("accessToken")] public string Token { get; set; }
    }
}
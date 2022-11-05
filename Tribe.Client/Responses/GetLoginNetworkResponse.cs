using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class GetLoginNetworkResponse
    {
        [JsonProperty("loginNetwork")] public AccessToken AccessToken { get; set; }
    }
}
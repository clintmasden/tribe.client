using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class GetNetworkDomainResponse
    {
        [JsonProperty("tokens")] public AccessToken AccessToken { get; set; }
    }
}
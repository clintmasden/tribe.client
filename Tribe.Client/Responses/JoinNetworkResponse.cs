using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class JoinNetworkResponse
    {
        [JsonProperty("joinNetwork")] public JoinResponse Response { get; set; }

        public class JoinResponse
        {
            public Member Member { get; set; }
        }
    }
}
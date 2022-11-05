using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class CreateReplyResponse
    {
        [JsonProperty("createReply")] public Post Post { get; set; }
    }
}
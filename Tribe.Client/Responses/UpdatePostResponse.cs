using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class UpdatePostResponse
    {
        [JsonProperty("updatePost")] public Post Post { get; set; }
    }
}
using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class CreatePostResponse
    {
        [JsonProperty("createPost")] public Post Post { get; set; }
    }
}
using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class CreateCollectionResponse
    {
        [JsonProperty("createCollection")] public TribeCollection Collection { get; set; }
    }
}
using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class CreateSpaceResponse
    {
        [JsonProperty("createSpace")] public Space Space { get; set; }
    }
}
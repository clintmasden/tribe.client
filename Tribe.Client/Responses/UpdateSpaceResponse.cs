using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class UpdateSpaceResponse
    {
        [JsonProperty("updateSpace")] public Space Space { get; set; }
    }
}
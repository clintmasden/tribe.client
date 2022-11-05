using Newtonsoft.Json;

namespace Tribe.Client.Models
{
    public class SpaceRole
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("description")] public string Description { get; set; }
    }
}
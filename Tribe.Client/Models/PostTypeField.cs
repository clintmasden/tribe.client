using Newtonsoft.Json;

namespace Tribe.Client.Models
{
    public class PostTypeField
    {
        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }
}
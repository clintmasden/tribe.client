using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tribe.Client.Models
{
    public class PostType
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("postFields")] public PostTypeFields Fields { get; set; }

        public class PostTypeFields
        {
            [JsonProperty("fields")] public List<PostTypeField> Fields { get; set; }
        }
    }
}
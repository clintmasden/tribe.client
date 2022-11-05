using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tribe.Client.Models
{
    public class MemberRole
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<string> Scopes { get; set; }

        public string? Type { get; set; }

        [JsonProperty("visible")] public bool? IsVisible { get; set; }
    }
}
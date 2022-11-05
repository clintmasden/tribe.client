using Newtonsoft.Json;

namespace Tribe.Client.Models
{
    public class SpaceMember
    {
        public Member Member { get; set; }

        public Space Space { get; set; }

        [JsonProperty("role")] public SpaceRole SpaceRole { get; set; }
    }
}
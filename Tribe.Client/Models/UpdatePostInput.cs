using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tribe.Client.Models
{
    public class UpdatePostInput
    {
        public UpdatePostInput()
        {
            MappingFields = new List<PostMappingFieldInput>();
        }

        //public List<string> AttachmentIds { get; set; }

        [JsonProperty("locked")] public bool IsLocked { get; set; }

        public List<PostMappingFieldInput> MappingFields { get; set; }

        public string OwnerId { get; set; }

        [JsonProperty("publish")] public bool IsPublished { get; set; }

        //public List<string> TagNames { get; set; }
    }
}
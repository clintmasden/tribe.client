using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tribe.Client.Models
{
    public class CreatePostInput
    {
        public CreatePostInput()
        {
            MappingFields = new List<PostMappingFieldInput>();
            CreatedAt = DateTimeOffset.Now;
        }

        // public List<string> AttachmentIds { get; set; }

        [JsonProperty("locked")] public bool IsLocked { get; set; }

        public List<PostMappingFieldInput> MappingFields { get; set; }

        public string OwnerId { get; set; }

        public string PostTypeId { get; set; }

        [JsonProperty("publish")] public bool IsPublished { get; set; }

        public List<string> TagIds { get; set; }

        //public List<string> TagNames { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tribe.Client.Models
{
    public class TribeCollection
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("createdBy")] public SpaceMember CreatedBy { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("description")] public object Description { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("createdAt")] public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("updatedAt")] public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty("spaces")] public CollectionSpaces Spaces { get; set; }

        public class CollectionSpaces
        {
            [JsonProperty("nodes")] public List<Space> Spaces { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }
    }
}
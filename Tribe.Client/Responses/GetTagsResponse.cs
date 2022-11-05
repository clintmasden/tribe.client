using Newtonsoft.Json;
using System.Collections.Generic;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class GetTagsResponse
    {
        [JsonProperty("tags")] public TagsResponse Response { get; set; }

        public class TagsResponse
        {
            [JsonProperty("nodes")] public List<Tag> Tags { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }
    }
}
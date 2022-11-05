using Newtonsoft.Json;
using System.Collections.Generic;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class GetPostTypesResponse
    {
        [JsonProperty("postTypes")] public PostTypesResponse Response { get; set; }

        public class PostTypesResponse
        {
            [JsonProperty("nodes")] public List<PostType> PostTypes { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }
    }
}
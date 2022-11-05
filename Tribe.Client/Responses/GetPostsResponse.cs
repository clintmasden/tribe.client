using Newtonsoft.Json;
using System.Collections.Generic;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class GetPostsResponse
    {
        [JsonProperty("posts")] public PostsResponse Response { get; set; }

        public class PostsResponse
        {
            [JsonProperty("nodes")] public List<Post> Posts { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }
    }
}
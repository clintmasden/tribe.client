using Newtonsoft.Json;
using System.Collections.Generic;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class GetMembersResponse
    {
        [JsonProperty("members")] public MembersResponse Response { get; set; }

        public class MembersResponse
        {
            [JsonProperty("nodes")] public List<Member> Members { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }
    }
}
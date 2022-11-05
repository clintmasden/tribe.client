using Newtonsoft.Json;
using System.Collections.Generic;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class GetSpacesResponse
    {
        [JsonProperty("spaces")] public SpacesResponse Response { get; set; }

        public class SpacesResponse
        {
            [JsonProperty("nodes")] public List<Space> Spaces { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }
    }
}
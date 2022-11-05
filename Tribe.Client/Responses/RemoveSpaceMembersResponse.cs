using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Tribe.Client.Responses
{
    public class RemoveSpaceMembersResponse
    {
        [JsonProperty("removeSpaceMembers")] public List<MutationResponse> Responses { get; set; }

        public bool IsSuccessful => Responses.All(r => r.Status.Equals("succeeded"));

        public class MutationResponse
        {
            public string Status { get; set; }
        }
    }
}
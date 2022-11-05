using Newtonsoft.Json;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class UpdateMemberResponse
    {
        [JsonProperty("updateMember")] public Member Member { get; set; }
    }
}
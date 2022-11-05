using Newtonsoft.Json;

namespace Tribe.Client.Responses
{
    public class UpdateCollectionResponse
    {
        [JsonProperty("updateCollection")] public MutationResponse Response { get; set; }

        public bool IsSuccessful => Response.Status.Equals("succeeded");

        public class MutationResponse
        {
            public string Status { get; set; }
        }
    }
}
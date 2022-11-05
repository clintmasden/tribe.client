using Newtonsoft.Json;

namespace Tribe.Client.Responses
{
    public class DeleteCollectionResponse
    {
        [JsonProperty("deleteCollection")] public MutationResponse Response { get; set; }

        public bool IsSuccessful => Response.Status.Equals("succeeded");

        public class MutationResponse
        {
            public string Status { get; set; }
        }
    }
}
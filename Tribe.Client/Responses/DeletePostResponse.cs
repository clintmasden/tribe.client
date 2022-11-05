using Newtonsoft.Json;

namespace Tribe.Client.Responses
{
    public class DeletePostResponse
    {
        [JsonProperty("deletePost")] public MutationResponse Response { get; set; }

        public bool IsSuccessful => Response.Status.Equals("succeeded");

        public class MutationResponse
        {
            public string Status { get; set; }
        }
    }
}
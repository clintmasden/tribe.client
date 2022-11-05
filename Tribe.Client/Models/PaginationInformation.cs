using Newtonsoft.Json;

namespace Tribe.Client.Models
{
    public class PaginationInformation
    {
        [JsonProperty("hasNextPage")] public bool HasNextPage { get; set; }
    }
}
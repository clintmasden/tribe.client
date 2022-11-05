using System.Collections.Generic;
using Tribe.Client.Models;

namespace Tribe.Client.Responses
{
    public class GetCollectionsResponse
    {
        public List<TribeCollection> Collections { get; set; }
    }
}
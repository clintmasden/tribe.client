using System.Collections.Generic;
using Tribe.Client.Models;

namespace Tribe.Client.Mutations
{
    public class AddSpaceMembersMutation
    {
        public List<AddSpaceMemberInput> Inputs { get; set; }

        public string SpaceId { get; set; }
    }
}
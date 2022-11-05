using System.Collections.Generic;

namespace Tribe.Client.Mutations
{
    public class RemoveSpaceMembersMutation
    {
        public List<string> MemberIds { get; set; }

        public string SpaceId { get; set; }
    }
}
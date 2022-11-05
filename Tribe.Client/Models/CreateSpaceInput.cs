using System;

namespace Tribe.Client.Models
{
    public class CreateSpaceInput
    {
        public CreateSpaceInput()
        {
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = DateTimeOffset.Now;
        }

        //public List<string> AdminIds { get; set; }

        public string CollectionId { get; set; }

        public string Description { get; set; }

        //[JsonProperty("hidden")] public bool IsHidden { get; set; }

        //public string ImageId { get; set; }

        //[JsonProperty("inviteOnly")] public bool IsInviteOnly { get; set; }

        //public string Layout { get; set; }

        //public List<string> MemberIds { get; set; }

        public string Name { get; set; }

        //[JsonProperty("nonAdminsCanInvite")] public bool CanMembersInvite { get; set; }

        //[JsonProperty("private")] public bool IsPrivate { get; set; }

        //public string Slug { get; set; }

        //[JsonProperty("type")] public string SpaceType { get; set; }

        //public string WhoCanPost { get; set; }

        //public string WhoCanReact { get; set; }

        //public string WhoCanReply { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
using System;

namespace Tribe.Client.Models
{
    public class UpdateMemberInput
    {
        public UpdateMemberInput()
        {
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = DateTimeOffset.Now;
        }

        public string BannerId { get; set; }

        public string CurrentPassword { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string ExternalId { get; set; }

        public string Locale { get; set; }

        public string Name { get; set; }

        public string NewPassword { get; set; }

        public string ProfilePictureId { get; set; }

        public string RoleId { get; set; }

        public string TagLine { get; set; }

        public string UserName { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
using Newtonsoft.Json;
using System;

namespace Tribe.Client.Models
{
    public class Member
    {
        public string Id { get; set; }

        public string RoleId { get; set; }

        [JsonProperty("role")] public MemberRole Role { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public DateTimeOffset? VerifiedAt { get; set; }

        public DateTimeOffset? LastSeenAt { get; set; }
    }
}
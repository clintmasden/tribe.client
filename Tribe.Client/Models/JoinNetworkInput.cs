using System;

namespace Tribe.Client.Models
{
    public class JoinNetworkInput
    {
        public JoinNetworkInput()
        {
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = DateTimeOffset.Now;
        }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        //public string Phone { get; set; }

        //public string UserName { get; set; }

        //[JsonProperty("verified")] public bool IsVerified { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
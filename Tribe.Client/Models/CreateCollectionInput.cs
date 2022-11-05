using System;

namespace Tribe.Client.Models
{
    public class CreateCollectionInput
    {
        public CreateCollectionInput()
        {
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = DateTimeOffset.Now;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
using Tribe.Client.Models;

namespace Tribe.Client.Mutations
{
    public class CreatePostMutation
    {
        public CreatePostInput Input { get; set; }

        public string SpaceId { get; set; }
    }
}
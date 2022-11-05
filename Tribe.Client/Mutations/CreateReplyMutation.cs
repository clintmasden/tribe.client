using Tribe.Client.Models;

namespace Tribe.Client.Mutations
{
    public class CreateReplyMutation
    {
        public CreatePostInput Input { get; set; }

        public string PostId { get; set; }
    }
}
namespace Tribe.Client.Models
{
    public class SingleSignOnUser
    {
        /// <summary>
        /// The Id in your product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email Address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public string TagLine { get; set; }
    }
}
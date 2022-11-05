using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tribe.Client.Models
{
    public class Post
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("createdById")] public string CreatedById { get; set; }

        [JsonProperty("createdBy")] public SpaceMember CreatedBy { get; set; }

        [JsonProperty("ownerId")] public string OwnerId { get; set; }

        [JsonProperty("owner")] public SpaceMember Owner { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("description")] public object Description { get; set; }

        [JsonProperty("shortContent")] public string ShortContent { get; set; }

        [JsonProperty("postTypeId")] public string PostTypeId { get; set; }

        [JsonProperty("postType")] public PostType PostType { get; set; }

        [JsonProperty("spaceId")] public string SpaceId { get; set; }

        [JsonProperty("space")] public Space Space { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("createdAt")] public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("updatedAt")] public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty("lastActivityAt")] public DateTimeOffset? LastActivityAt { get; set; }

        [JsonProperty("publishedAt")] public DateTimeOffset? PublishedAt { get; set; }

        public int ReactionCount { get; set; }

        public List<PostReaction> Reactions { get; set; }

        public int RepliesCount { get; set; }

        [JsonProperty("replies")] public PostReplies Replies { get; set; }

        public class PostReplies
        {
            [JsonProperty("nodes")] public List<Post> Replies { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }
    }
}
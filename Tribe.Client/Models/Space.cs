using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tribe.Client.Models
{
    public class Space
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("createdById")] public string CreatedById { get; set; }

        [JsonProperty("createdBy")] public Member CreatedBy { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("description")] public object Description { get; set; }

        [JsonProperty("groupId")] public string GroupId { get; set; }

        [JsonProperty("MembersCount")] public int MembersCount { get; set; }

        [JsonProperty("members")] public MemberNodes Members { get; set; }

        [JsonProperty("networkId")] public string NetworkId { get; set; }

        public Network Network { get; set; }

        public int PostsCount { get; set; }

        [JsonProperty("posts")] public PostNodes Posts { get; set; }

        [JsonProperty("Slug")] public string Slug { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("createdAt")] public DateTimeOffset? CreatedAt { get; set; }

        public class MemberNodes
        {
            [JsonProperty("nodes")] public List<SpaceMember> Members { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }

        public class PostNodes
        {
            [JsonProperty("nodes")] public List<Post> Posts { get; set; }

            [JsonProperty("pageInfo")] public PaginationInformation PaginationInformation { get; set; }

            [JsonProperty("totalCount")] public int Count { get; set; }
        }
    }
}
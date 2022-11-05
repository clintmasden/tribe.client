using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tribe.Client.Exceptions;
using Tribe.Client.Models;
using Tribe.Client.Mutations;
using Tribe.Client.Responses;

namespace Tribe.Client
{
    /// <summary>
    /// Creates client for accessing Tribe's endpoints.
    /// </summary>
    public sealed class TribeClient
    {
        /// <summary>
        /// Creates client for accessing Tribe's endpoints.
        /// </summary>
        public TribeClient(string networkDomain)
        {
            _graphQlHttpClient = new GraphQLHttpClient("https://app.tribe.so/graphql", new NewtonsoftJsonSerializer());
            _networkDomain = networkDomain;
        }

        /// <summary>
        /// Creates client for accessing Tribe's endpoints.
        /// </summary>
        public TribeClient(string networkDomain, string token) : this(networkDomain)
        {
            _graphQlHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        private GraphQLHttpClient _graphQlHttpClient { get; }

        private string _networkDomain { get; set; }

        /// <summary>
        ///     Helper method for throwing all errors reported from the response.
        /// </summary>
        private void ThrowResponseErrors(GraphQLError[] graphQlErrors)
        {
            if (graphQlErrors is null || !graphQlErrors.Any())
            {
                return;
            }

            throw new Exception(string.Join(Environment.NewLine, graphQlErrors.Select(x => $"{nameof(x.Path)}: {x.Path} | {nameof(x.Extensions)}: {x.Extensions} | {nameof(x.Message)}: {x.Message}").ToArray()));
        }

        /// <summary>
        /// Gets a guest access token <see href="https://partners.tribe.so/docs/guide/graphql/authentication/tribe-access-token">See Docs</see>
        /// </summary>
        public async Task<string> GetGuestAccessToken(string networkDomain, CancellationToken cancellationToken = default)
        {
            var graphQlHttpClient = new GraphQLHttpClient("https://app.tribe.so/graphql", new NewtonsoftJsonSerializer());

            var request = new GraphQLRequest
            {
                Query = @"query request($networkDomain: String) {
                      tokens(networkDomain: $networkDomain) {
                        accessToken
                      }
                    }",
                Variables = new
                {
                    networkDomain = networkDomain
                }
            };

            var result = await graphQlHttpClient.SendQueryAsync<GetNetworkDomainResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.AccessToken.Token;
        }

        /// <summary>
        /// Gets a network access token by username or email and password <see href="https://partners.tribe.so/docs/guide/graphql/authentication/tribe-access-token">See Docs</see>
        /// </summary>
        public async Task<string> GetAccessTokenByUsernameOrEmailAndPassword(string guestAccessToken, string usernameOrEmail, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(guestAccessToken))
            {
                throw new AccessTokenException("A guest access token must be provided.");
            }

            _graphQlHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", guestAccessToken);

            var request = new GraphQLRequest
            {
                Query = @"mutation request($usernameOrEmail: String!, $password: String!) {
                      loginNetwork(
                        input: { usernameOrEmail: $usernameOrEmail, password: $password }
                      ) {
                        accessToken
                      }
                    }",
                Variables = new
                {
                    usernameOrEmail = usernameOrEmail,
                    password = password
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<GetLoginNetworkResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.AccessToken.Token;
        }

        /// <summary>
        /// Sets access token for client.
        /// </summary>
        /// <remarks>
        /// First calls <see cref="GetGuestAccessToken">GuestAccessToken</see> then <see cref="GetAccessTokenByUsernameOrEmailAndPassword">AccessTokenByUsernameOrEmail</see>.
        /// </remarks>
        /// <exception cref="AccessTokenException"></exception>
        public async Task SetAccessToken(string usernameOrEmail, string password, CancellationToken cancellationToken = default)
        {
            string guestAccessToken;
            try
            {
                guestAccessToken = await GetGuestAccessToken(_networkDomain, cancellationToken);
            }
            catch
            {
                throw new AccessTokenException("Could not generate guest access token, verify that the network domain is correct.");
            }

            string usernameorEmailAccessToken;
            try
            {
                usernameorEmailAccessToken = await GetAccessTokenByUsernameOrEmailAndPassword(guestAccessToken, usernameOrEmail, password, cancellationToken);
            }
            catch
            {
                throw new AccessTokenException("Username, Email or Password incorrect.");
            }

            _graphQlHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", usernameorEmailAccessToken);
        }

        /// <summary>
        /// Get members.
        /// </summary>
        public async Task<List<Member>> GetMembers(int limit = 100, int offset = 0, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($limit: Int!, $offset: Int) {
                      members(limit: $limit, offset: $offset) {
                        nodes {
                          id
                          roleId
                          role {
                            id
                            name
                          }
                          username
                          email
                          name
                          status
                          createdAt
                          updatedAt
                        }
                        pageInfo {
                          hasNextPage
                        }
                        totalCount
                      }
                    }",
                Variables = new
                {
                    limit = limit,
                    offset = offset
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetMembersResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Response.Members;
        }

        /// <summary>
        /// Get a member.
        /// </summary>
        public async Task<Member> GetMember(string id, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($id: ID!) {
                      member(id: $id) {
                        id
                        roleId
                        role {
                          id
                          name
                          description
                          scopes
                          type
                          visible
                        }
                        username
                        email
                        name
                        status
                        createdAt
                        updatedAt
                        verifiedAt
                        lastSeenAt
                      }
                    }",
                Variables = new
                {
                    id = id,
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetMemberResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Member;
        }

        /// <summary>
        /// Create a member. <see href="https://partners.tribe.so/docs/guide/tutorials/create-member">See Docs</see>
        /// </summary>
        /// <remarks>
        /// Mutation: joinNetwork
        /// </remarks>
        public async Task<string> CreateMember(CreateMemberMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: JoinNetworkInput!) {
                      joinNetwork(input: $input) {
                        member {
                          id
                        }
                      }
                    }",
                Variables = new
                {
                    input = mutation.Input,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<JoinNetworkResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Response.Member.Id;
        }

        /// <summary>
        /// Update a member.
        /// </summary>
        /// <remarks>
        /// Unable to implement or verify || HTTP Bad Request or Internal Server Error
        /// </remarks>
        public async Task<string> UpdateMember(UpdateMemberMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: UpdateMemberInput!, $id: ID!) {
                          updateMember(input: $input, id: $id) {
                            id
                          }
                        }",
                Variables = new
                {
                    input = mutation.Input,
                    id = mutation.Id,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<UpdateMemberResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Member.Id;
        }

        /// <summary>
        /// Delete a member.
        /// </summary>
        public async Task<bool> DeleteMember(DeleteMemberMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($id: ID!) {
                      deleteMember(id: $id) {
                        status
                      }
                    }",
                Variables = new
                {
                    id = mutation.Id,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<DeleteMemberResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.IsSuccessful;
        }

        /// <summary>
        /// Add members to a space. <see href="https://partners.tribe.so/docs/guide/tutorials/space-members#add-members-to-space">See Docs</see>
        /// </summary>
        /// <remarks>
        /// Mutation: addSpaceMembers
        /// </remarks>
        public async Task<bool> AddMembersToSpace(AddSpaceMembersMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: [AddSpaceMemberInput!]!, $spaceId: ID!) {
                      addSpaceMembers(input: $input, spaceId: $spaceId) {
                        member {
                          email
                        }
                      }
                    }",
                Variables = new
                {
                    input = mutation.Inputs,
                    spaceId = mutation.SpaceId,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<dynamic>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            // the mutation return is irrelevant
            return true;
        }

        /// <summary>
        /// Remove members from a space. <see href="https://partners.tribe.so/docs/guide/tutorials/space-members#remove-members-from-space">See Docs</see>
        /// </summary>
        /// <remarks>
        /// Mutation: removeSpaceMembers
        /// </remarks>
        public async Task<bool> RemoveMembersFromSpace(RemoveSpaceMembersMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($memberIds: [ID!]!, $spaceId: ID!) {
                      removeSpaceMembers(memberIds: $memberIds, spaceId: $spaceId) {
                        status
                      }
                    }",
                Variables = new
                {
                    memberIds = mutation.MemberIds,
                    spaceId = mutation.SpaceId,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<RemoveSpaceMembersResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.IsSuccessful;
        }

        /// <summary>
        /// Get tags.
        /// </summary>
        public async Task<List<Tag>> GetTags(int limit = 100, int offset = 0, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($limit: Int!, $offset: Int) {
                      tags(limit: $limit, offset: $offset) {
                        nodes {
                          id
                          title
                          description
                          slug
                        }
                        pageInfo {
                          hasNextPage
                        }
                        totalCount
                      }
                    }",
                Variables = new
                {
                    limit = limit,
                    offset = offset
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetTagsResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Response.Tags;
        }

        /// <summary>
        /// Get posts.
        /// </summary>
        public async Task<List<Post>> GetPosts(int limit = 100, int offset = 0, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($limit: Int!, $offset: Int) {
                      posts(limit: $limit, offset: $offset) {
                        nodes {
                          id
                          createdById
                          createdBy {
                            member {
                              id
                              name
                            }
                          }
                          ownerId
                          owner {
                            member {
                              id
                              name
                            }
                          }
                          title
                          description
                          shortContent
                          postTypeId
                          postType {
                            id
                            name
                          }
                          spaceId
                          space {
                            id
                            name
                          }
                          status
                          createdAt
                          updatedAt
                          url
                        }
                        pageInfo {
                          hasNextPage
                        }
                        totalCount
                      }
                    }",
                Variables = new
                {
                    limit = limit,
                    offset = offset
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetPostsResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Response.Posts;
        }

        /// <summary>
        /// Get a post.
        /// </summary>
        public async Task<Post> GetPost(string id, int replyLimit = 10000, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($id: ID!, $replyLimit: Int!) {
                      post(id: $id) {
                        id
                        createdById
                        createdBy {
                          member {
                            id
                            roleId
                            role {
                              id
                              name
                              description
                              scopes
                              type
                              visible
                            }
                            username
                            email
                            name
                            status
                            createdAt
                            updatedAt
                            verifiedAt
                            lastSeenAt
                          }
                          role {
                            id
                            name
                            description
                          }
                          space {
                            id
                            name
                            description
                          }
                        }
                        ownerId
                        owner {
                          member {
                            id
                            roleId
                            role {
                              id
                              name
                              description
                              scopes
                              type
                              visible
                            }
                            username
                            email
                            name
                            status
                            createdAt
                            updatedAt
                            verifiedAt
                            lastSeenAt
                          }
                          role {
                            id
                            name
                            description
                          }
                          space {
                            id
                            name
                            description
                          }
                        }
                        title
                        description
                        shortContent
                        postTypeId
                        postType {
                          id
                          name
                        }
                        spaceId
                        space {
                          id
                          name
                        }
                        status
                        url
                        createdAt
                        updatedAt
                        lastActivityAt
                        publishedAt
                        reactionsCount
                        reactions {
                          count
                          reaction
                        }
                        repliesCount
                        replies(limit: $replyLimit) {
                          nodes {
                            id
                            createdById
                            ownerId
                            title
                            description
                            shortContent
                            status
                            createdAt
                            updatedAt
                            url
                          }
                          pageInfo {
                            hasNextPage
                          }
                          totalCount
                        }
                      }
                    }",
                Variables = new
                {
                    id = id,
                    replyLimit = replyLimit
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetPostResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Post;
        }

        /// <summary>
        /// Get the feed.
        /// </summary>
        public async Task<List<Post>> GetFeed(int limit = 100, int offset = 0, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($limit: Int!, $offset: Int) {
                      feed(limit: $limit, offset: $offset) {
                        nodes {
                          id
                          createdById
                          createdBy {
                            member {
                              id
                              name
                            }
                          }
                          ownerId
                          owner {
                            member {
                              id
                              name
                            }
                          }
                          title
                          description
                          shortContent
                          postTypeId
                          postType {
                            id
                            name
                          }
                          spaceId
                          space {
                            id
                            name
                          }
                          status
                          createdAt
                          updatedAt
                          url
                        }
                        pageInfo {
                          hasNextPage
                        }
                        totalCount
                      }
                    }",
                Variables = new
                {
                    limit = limit,
                    offset = offset
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetFeedResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Response.Posts;
        }

        /// <summary>
        /// Get post types.
        /// </summary>
        public async Task<List<PostType>> GetPostTypes(int limit = 100, int offset = 0, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($limit: Int!, $offset: Int) {
                      postTypes(limit: $limit, offset: $offset) {
                        nodes {
                          id
                          name
                          postFields {
                            fields {
                              key
                              name
                              type
                            }
                          }
                        }
                        pageInfo {
                          hasNextPage
                        }
                        totalCount
                      }
                    }",
                Variables = new
                {
                    limit = limit,
                    offset = offset
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetPostTypesResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Response.PostTypes;
        }

        /// <summary>
        /// Create a post. <see href="https://partners.tribe.so/docs/guide/tutorials/create-post">See Docs</see>
        /// </summary>
        public async Task<string> CreatePost(CreatePostMutation mutation)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: CreatePostInput!, $spaceId: ID!) {
                      createPost(input: $input, spaceId: $spaceId) {
                        id
                      }
                    }",
                Variables = new
                {
                    input = mutation.Input,
                    spaceId = mutation.SpaceId,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<CreatePostResponse>(request);

            ThrowResponseErrors(result.Errors);

            return result.Data.Post.Id;
        }

        /// <summary>
        /// Create a reply. <see href="https://partners.tribe.so/docs/guide/tutorials/create-post/#reply-to-post">See Docs</see>
        /// </summary>
        public async Task<string> CreateReply(CreateReplyMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: CreatePostInput!, $postId: ID!) {
                      createReply(input: $input, postId: $postId) {
                        id
                      }
                    }",
                Variables = new
                {
                    input = mutation.Input,
                    postId = mutation.PostId,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<CreateReplyResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Post.Id;
        }

        /// <summary>
        /// Updates a post. Unable to determine issue / error here.
        /// </summary>
        /// <remarks>
        /// Unable to implement or verify || HTTP Bad Request or Internal Server Error
        /// </remarks>
        public async Task<string> UpdatePost(UpdatePostMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: UpdatePostInput!, $id: ID!) {
                          updatePost(input: $input, id: $id) {
                            id
                          }
                        }",
                Variables = new
                {
                    input = mutation.Input,
                    id = mutation.Id,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<UpdatePostResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Post.Id;
        }

        /// <summary>
        /// Delete a post.
        /// </summary>
        public async Task<bool> DeletePost(DeletePostMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($id: ID!) {
                      deletePost(id: $id) {
                        status
                      }
                    }",
                Variables = new
                {
                    id = mutation.Id,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<DeletePostResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.IsSuccessful;
        }

        /// <summary>
        /// Get collections.
        /// </summary>
        public async Task<List<TribeCollection>> GetCollections(int limit = 100, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($limit: Int!) {
                      collections(reverse: false) {
                        id
                        name
                        description
                        url
                        createdAt
                        createdBy {
                          id
                          name
                        }
                        updatedAt
                        spaces(limit: $limit) {
                          nodes {
                            id
                            name
                          }
                          pageInfo {
                            hasNextPage
                          }
                          totalCount
                        }
                      }
                    }",
                Variables = new
                {
                    limit = limit
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetCollectionsResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Collections;
        }

        /// <summary>
        /// Get a collection.
        /// </summary>
        public async Task<TribeCollection> GetCollection(string id, int limit = 100, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($id: ID!, $limit: Int!) {
                      collection(id: $id) {
                        id
                        name
                        description
                        url
                        createdAt
                        createdBy {
                          id
                          name
                        }
                        updatedAt
                        spaces(limit: $limit) {
                          nodes {
                            id
                            name
                          }
                          pageInfo {
                            hasNextPage
                          }
                          totalCount
                        }
                      }
                    }",
                Variables = new
                {
                    id = id,
                    limit = limit
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetCollectionResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Collection;
        }

        /// <summary>
        /// Create a collection. <see href="https://partners.tribe.so/docs/guide/tutorials/create-space#create-a-space-inside-a-collection">See Docs</see>
        /// </summary>
        public async Task<string> CreateCollection(CreateCollectionMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: CreateCollectionInput!) {
                      createCollection(input: $input) {
                        id
                      }
                    }",
                Variables = new
                {
                    input = mutation.Input,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<CreateCollectionResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Collection.Id;
        }

        /// <summary>
        /// Update a collection.
        /// </summary>
        /// <remarks>
        /// Unable to implement or verify || HTTP Bad Request or Internal Server Error
        /// </remarks>
        public async Task<bool> UpdateCollection(UpdateCollectionMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: UpdateCollectionInput!, $id: ID!) {
                          updateCollection(input: $input, id: $id) {
                            id
                          }
                        }",
                Variables = new
                {
                    input = mutation.Input,
                    id = mutation.Id,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<UpdateCollectionResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.IsSuccessful;
        }

        /// <summary>
        /// Delete a collection.
        /// </summary>
        public async Task<bool> DeleteCollection(DeleteCollectionMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($id: ID!) {
                      deleteCollection(id: $id) {
                        status
                      }
                    }",
                Variables = new
                {
                    id = mutation.Id,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<DeleteCollectionResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.IsSuccessful;
        }

        /// <summary>
        /// Get spaces. <see href="https://partners.tribe.so/docs/guide/tutorials/retrieve-spaces#list-of-spaces">See Docs</see>
        /// </summary>
        public async Task<dynamic> GetSpaces(int limit = 100, int offset = 0, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($limit: Int!, $offset: Int) {
                      spaces(limit: $limit, offset: $offset) {
                        nodes {
                          id
                          createdById
                          createdBy {
                            id
                            name
                          }
                          name
                          description
                          groupId
                          membersCount
                          networkId
                          network {
                            id
                            ownerId
                            name
                          }
                          postsCount
                          slug
                          type
                          createdAt
                          url
                        }
                        pageInfo {
                          hasNextPage
                        }
                        totalCount
                      }
                    }",
                Variables = new
                {
                    limit = limit,
                    offset = offset
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetSpacesResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Response.Spaces;
        }

        /// <summary>
        /// Get a space. <see href="https://partners.tribe.so/docs/guide/tutorials/retrieve-spaces#retrieve-information-about-a-space">See Docs</see>
        /// </summary>
        public async Task<dynamic> GetSpace(string id, int memberLimit = 10000, int postLimit = 10000, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"query request($id: ID!, $memberLimit: Int!, $postLimit: Int!) {
                      space(id: $id) {
                        id
                        createdById
                        createdBy {
                          id
                          name
                        }
                        name
                        description
                        groupId
                        membersCount
                        members(limit: $memberLimit) {
                          nodes {
                            member {
                              id
                              roleId
                              username
                              email
                              name
                              status
                              createdAt
                              updatedAt
                            }
                            role {
                              id
                              name
                              description
                            }
                            space {
                              id
                              name
                              description
                            }
                          }
                          pageInfo {
                            hasNextPage
                          }
                          totalCount
                        }
                        networkId
                        network {
                          id
                          ownerId
                          name
                        }
                        postsCount
                        posts(limit: $postLimit) {
                          nodes {
                            id
                            createdById
                            ownerId
                            title
                            description
                            shortContent
                            status
                            createdAt
                            updatedAt
                            url
                          }
                          pageInfo {
                            hasNextPage
                          }
                          totalCount
                        }
                        slug
                        type
                        createdAt
                        url
                      }
                    }",
                Variables = new
                {
                    id = id,
                    memberLimit = memberLimit,
                    postLimit = postLimit
                }
            };

            var result = await _graphQlHttpClient.SendQueryAsync<GetSpaceResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Space;
        }

        /// <summary>
        /// Create a space. <see href="https://partners.tribe.so/docs/guide/tutorials/create-space">See Docs</see>
        /// </summary>
        public async Task<string> CreateSpace(CreateSpaceMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: CreateSpaceInput!) {
                      createSpace(input: $input) {
                        id
                      }
                    }",
                Variables = new
                {
                    input = mutation.Input,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<CreateSpaceResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Space.Id;
        }

        /// <summary>
        /// Update a space.
        /// </summary>
        /// <remarks>
        /// Unable to implement or verify || HTTP Bad Request or Internal Server Error
        /// </remarks>
        public async Task<string> UpdateSpace(UpdateSpaceMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($input: UpdateSpaceInput!, $id: ID!) {
                          updateSpace(input: $input, id: $id) {
                            id
                          }
                        }",
                Variables = new
                {
                    input = mutation.Input,
                    id = mutation.Id,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<UpdateSpaceResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.Space.Id;
        }

        /// <summary>
        /// Delete a space.
        /// </summary>
        public async Task<bool> DeleteSpace(DeleteSpaceMutation mutation, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation request($id: ID!) {
                      deleteSpace(id: $id) {
                        status
                      }
                    }
                    ",
                Variables = new
                {
                    id = mutation.Id,
                }
            };

            var result = await _graphQlHttpClient.SendMutationAsync<DeleteSpaceResponse>(request, cancellationToken);

            ThrowResponseErrors(result.Errors);

            return result.Data.IsSuccessful;
        }

        /// <summary>
        ///    A custom query or mutation using the <see cref="_graphQlHttpClient">GraphQL Client</see>.
        /// </summary>
        public async Task<T> CustomQueryOrMutation<T>(string queryOrMutation, CancellationToken cancellationToken = default)
        {
            return await CustomQueryOrMutation<T>(queryOrMutation, null, cancellationToken);
        }

        /// <summary>
        ///    A custom query or mutation using the <see cref="_graphQlHttpClient">GraphQL Client</see>.
        /// </summary>
        public async Task<T> CustomQueryOrMutation<T>(string queryOrMutation, object variables, CancellationToken cancellationToken = default)
        {
            var request = new GraphQLRequest
            {
                Query = queryOrMutation
            };

            if (variables != null)
            {
                request.Variables = variables;
            }

            var result = await _graphQlHttpClient.SendMutationAsync<T>(request);

            ThrowResponseErrors(result.Errors);

            return result.Data;
        }

        /// <summary>
        /// Creates and returns the client while <see cref="SetAccessToken">setting the access token</see>.
        /// </summary>
        public static async Task<TribeClient> CreateAsync(string apiUrl, string usernameOrEmail, string password)
        {
            var client = new TribeClient(apiUrl);
            await client.SetAccessToken(usernameOrEmail, password);

            return client;
        }

        /// <summary>
        /// Gets a single sign on json web token and launches your default web browser. <see href="https://partners.tribe.so/docs/guide/single-sign-on/jwt-sso">See Docs</see>
        /// </summary>
        public void OpenWebBrowserForSingleSignOnWithJsonWebToken(string ssoUrl, string jwtSecretKey, SingleSignOnUser user)
        {
            // https://YOUR_COMMUNITY_DOMAIN/api/auth/sso?jwt=GetSingleSignOnJsonWebToken&redirect_uri=
            ssoUrl = ssoUrl.EndsWith("?jwt=") ? ssoUrl : $"{ssoUrl}?jwt=";

            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = $"{ssoUrl}{GetSingleSignOnJsonWebToken(jwtSecretKey, user)}"
            };
            System.Diagnostics.Process.Start(processStartInfo);
        }

        /// <summary>
        /// Gets a single sign on json web token. <see href="https://partners.tribe.so/docs/guide/single-sign-on/jwt-sso">See Docs</see>
        /// </summary>
        /// <returns></returns>
        public string GetSingleSignOnJsonWebToken(string jwtSecretKey, SingleSignOnUser user)
        {
            if (string.IsNullOrWhiteSpace(jwtSecretKey))
            {
                // https://YOUR_COMMUNITY_DOMAIN/manage/settings/authentication
                throw new AccessTokenException("The JWT private key must be provided.");
            }

            var jwtPayload = new JwtPayload
            {
                { "sub", user.Id },
                { "email", user.Email },
                { "name", user.Name },
                { "tagline", user.TagLine }
            };

            var securityToken = new JwtSecurityToken(new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)), "HS256")), jwtPayload);
            return (new JwtSecurityTokenHandler()).WriteToken(securityToken);
        }
    }
}
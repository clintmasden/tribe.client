using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tribe.Client;
using Tribe.Client.Models;
using Tribe.Client.Responses;
using Tribe.Tests.Models;
using Xunit;

namespace Tribe.Tests
{
    public class TribeClientTests
    {
        private TribeClientConfiguration _configuration { get; set; }

        private TribeClient _client { get; set; }

        public TribeClientTests()
        {
            var relativeFilePath = "../../../../tribeClientConfiguration.json";

            _configuration = new TribeClientConfiguration()
            {
                NetworkDomain = "community.domain.com",
                AccessToken = string.Empty,
                UsernameOrEmail = "aUser1234!@gssmail.com",
                Password = "aPassword1234!",
                SingleSignOnUrl = "https://domain.tribeplatform.com/api/auth/sso?jwt=",
                JwtSecretKey = string.Empty
            };

            //File.WriteAllText(relativeFilePath, JsonConvert.SerializeObject(_configuration));

            if (File.Exists(relativeFilePath))
            {
                _configuration = JsonConvert.DeserializeObject<TribeClientConfiguration>(File.ReadAllText(relativeFilePath));
            }

            _client = new TribeClient(_configuration.NetworkDomain, _configuration.AccessToken);
        }

        [Fact]
        public async Task GetGuestAccessToken_Pass()
        {
            var result = await _client.GetGuestAccessToken(_configuration.NetworkDomain);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAccessTokenByUsernameOrEmailAndPassword_Pass()
        {
            var guestAccessToken = await _client.GetGuestAccessToken(_configuration.NetworkDomain);
            Assert.NotNull(guestAccessToken);

            var result = await _client.GetAccessTokenByUsernameOrEmailAndPassword(guestAccessToken, _configuration.UsernameOrEmail, _configuration.Password);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SetAccessToken_Pass()
        {
            await _client.SetAccessToken(_configuration.UsernameOrEmail, _configuration.Password);
        }

        [Fact]
        public async Task CrudMembers_Pass()
        {
            // create
            var createId = await _client.CreateMember(new()
            {
                Input = new()
                {
                    Name = "Create Functional Name",
                    //UserName = userName,
                    Email = "email@functoinal.com",
                    Password = "Create Functional Password!",
                    //Phone = "8675309"
                }
            });

            Assert.NotNull(createId);

            // update
            //var updateId = await _client.UpdateMember(new()
            //{
            //    Id = createId,
            //    Input = new()
            //    {
            //        Name = "Update Functional Name",
            //    }
            //});

            //Assert.NotEmpty(updateId);

            // read / get
            var entities = await _client.GetMembers();
            Assert.NotEmpty(entities);

            var entity = await _client.GetMember(createId);
            Assert.NotNull(entity);

            // delete
            var deleteResult = await _client.DeleteMember(new()
            {
                Id = createId
            });

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task CrudMembersToSpace_Pass()
        {
            // dependencies
            var members = await _client.GetMembers();
            Assert.NotEmpty(members);

            var firstMemberId = members.First().Id;

            var createSpaceId = await _client.CreateSpace(new()
            {
                Input = new()
                {
                    Name = "Create Functional Name",
                    Description = "Create Functional Description",
                }
            });

            Assert.NotNull(createSpaceId);

            // add
            var addMemberResult = await _client.AddMembersToSpace(new()
            {
                SpaceId = createSpaceId,
                Inputs = new()
                {
                    new() {MemberId = firstMemberId }
                }
            });

            Assert.True(addMemberResult);

            // remove
            var removeMembersResult = await _client.RemoveMembersFromSpace(new()
            {
                MemberIds = new() { firstMemberId },
                SpaceId = createSpaceId
            });

            Assert.True(removeMembersResult);

            // dependencies
            var deleteSpaceResult = await _client.DeleteSpace(new()
            {
                Id = createSpaceId
            });

            Assert.True(deleteSpaceResult);
        }

        [Fact]
        public async Task CrudCollection_Pass()
        {
            // create
            var createId = await _client.CreateCollection(new()
            {
                Input = new()
                {
                    Name = "Create Functional Name",
                    Description = "Create Functional Description"
                }
            });

            Assert.NotNull(createId);

            // update
            //var updateResult = await _client.UpdateCollection(new()
            //{
            //    Id = createId,
            //    Input = new()
            //    {
            //        Name = "Update Functional Name",
            //        Description = "Update Functional Description"
            //    }
            //});

            //Assert.True(updateResult);

            // read / get
            var entities = await _client.GetCollections();
            Assert.NotEmpty(entities);

            var entity = await _client.GetCollection(createId);
            Assert.NotNull(entity);

            // delete
            var deleteResult = await _client.DeleteCollection(new()
            {
                Id = createId
            });

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task CrudSpace_Pass()
        {
            // dependencies
            var createCollectionId = await _client.CreateCollection(new()
            {
                Input = new()
                {
                    Name = "Create Functional Name",
                    Description = "Create Functional Description"
                }
            });

            // create
            var createId = await _client.CreateSpace(new()
            {
                Input = new()
                {
                    Name = "Create Functional Name",
                    Description = "Create Functional Description",
                    CollectionId = createCollectionId
                }
            });

            Assert.NotNull(createId);

            // update
            //var updateResult = await _client.UpdateSpace(new()
            //{
            //    Id = createId,
            //    Input = new()
            //    {
            //        Name = "Update Functional Name",
            //        Description = "Update Functional Description"
            //    }
            //});

            //Assert.NotEmpty(updateResult);

            // read / get
            var entities = await _client.GetSpaces();
            Assert.NotEmpty(entities);

            var entity = await _client.GetSpace(createId);
            Assert.NotNull(entity);

            // delete
            var deleteResult = await _client.DeleteSpace(new()
            {
                Id = createId
            });

            Assert.True(deleteResult);

            // dependencies
            var deleteCollectionResult = await _client.DeleteCollection(new()
            {
                Id = createCollectionId
            });

            Assert.True(deleteCollectionResult);
        }

        [Fact]
        public async Task GetFeed_Pass()
        {
            var entities = await _client.GetFeed();
            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task GetPostTypes_Pass()
        {
            var entities = await _client.GetPostTypes();
            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task GetTags_Pass()
        {
            var entities = await _client.GetTags();
            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task CrudPost_Pass()
        {
            // dependencies
            var members = await _client.GetMembers();
            Assert.NotEmpty(members);

            var firstMemberId = members.First().Id;

            var postTypes = await _client.GetPostTypes();
            Assert.NotEmpty(postTypes);

            var discussionPostType = postTypes.SingleOrDefault(c => c.Name.Equals("discussion", StringComparison.OrdinalIgnoreCase));
            Assert.NotNull(discussionPostType);

            var createSpaceId = await _client.CreateSpace(new()
            {
                Input = new()
                {
                    Name = "Create Functional Name",
                    Description = "Create Functional Description"
                }
            });
            Assert.NotNull(createSpaceId);

            // create
            var createId = await _client.CreatePost(new()
            {
                SpaceId = createSpaceId,
                Input = new()
                {
                    OwnerId = firstMemberId,
                    PostTypeId = discussionPostType.Id,
                    IsPublished = true,
                    IsLocked = true,
                    CreatedAt = DateTimeOffset.Now.AddMinutes(-15),
                    MappingFields = new()
                        {
                            new()
                            {
                                Key = "title",
                                MappingType = "text",
                                Value ="Create Functional Title"
                                //Value ="\"Mutation Title\""
                            },
                            new()
                            {
                                Key = "content",
                                MappingType = "html",
                                Value = "<p>Create Functional Content</p>"
                            }
                        }
                }
            });

            Assert.NotNull(createId);

            // update
            //var updateId = await _client.UpdatePost(new()
            //{
            //    Id = createId,
            //    Input = new()
            //    {
            //        OwnerId = firstMemberId,
            //        IsPublished = true,
            //        MappingFields = new()
            //            {
            //                new()
            //                {
            //                    Key = "title",
            //                    MappingType = "text",
            //                    Value ="Update Functional Title"
            //                }
            //            }
            //    }
            //});

            //Assert.NotEmpty(updateId);

            // read / get
            var entities = await _client.GetPosts();
            Assert.NotEmpty(entities);

            var entity = await _client.GetPost(createId);
            Assert.NotNull(entity);

            // delete
            var deleteResult = await _client.DeletePost(new()
            {
                Id = createId
            });

            Assert.True(deleteResult);

            // dependencies
            var deleteSpaceResult = await _client.DeleteSpace(new()
            {
                Id = createSpaceId
            });

            Assert.True(deleteSpaceResult);
        }

        [Fact]
        public async Task CrudPostReply_Pass()
        {
            // dependencies
            var members = await _client.GetMembers();
            Assert.NotEmpty(members);

            var firstMemberId = members.First().Id;

            var postTypes = await _client.GetPostTypes();
            Assert.NotEmpty(postTypes);

            var discussionPostType = postTypes.SingleOrDefault(c => c.Name.Equals("discussion", StringComparison.OrdinalIgnoreCase));
            Assert.NotNull(discussionPostType);

            var commentPostType = postTypes.SingleOrDefault(c => c.Name.Equals("comment", StringComparison.OrdinalIgnoreCase));
            Assert.NotNull(commentPostType);

            var createSpaceId = await _client.CreateSpace(new()
            {
                Input = new()
                {
                    Name = "Create Functional Name",
                    Description = "Create Functional Description"
                }
            });
            Assert.NotNull(createSpaceId);

            var createPostId = await _client.CreatePost(new()
            {
                SpaceId = createSpaceId,
                Input = new()
                {
                    OwnerId = firstMemberId,
                    PostTypeId = discussionPostType.Id,
                    IsPublished = true,
                    IsLocked = true,
                    CreatedAt = DateTimeOffset.Now.AddMinutes(-15),
                    MappingFields = new()
                        {
                            new()
                            {
                                Key = "title",
                                MappingType = "text",
                                Value ="Create Functional Title"
                            },
                            new()
                            {
                                Key = "content",
                                MappingType = "html",
                                Value = "<p>Create Functional Content</p>"
                            }
                        }
                }
            });

            Assert.NotNull(createPostId);

            // create
            var createId = await _client.CreateReply(new()
            {
                PostId = createPostId,
                Input = new()
                {
                    OwnerId = firstMemberId,
                    PostTypeId = commentPostType.Id,
                    IsPublished = true,
                    MappingFields = new()
                        {
                            new()
                            {
                                Key = "content",
                                MappingType = "html",
                                Value ="Create Functional Content"
                            }
                        }
                }
            });

            // update
            var updateId = await _client.UpdatePost(new()
            {
                Id = createId,
                Input = new()
                {
                    OwnerId = firstMemberId,
                    IsPublished = true,
                    MappingFields = new()
                       {
                            new()
                            {
                                Key = "content",
                                MappingType = "html",
                                Value ="Update Functional Content"
                            }
                       }
                }
            });

            Assert.NotNull(updateId);

            // read / get
            var entity = await _client.GetPost(createId);
            Assert.NotNull(entity);

            // delete
            var deleteResult = await _client.DeletePost(new()
            {
                Id = createId
            });

            Assert.True(deleteResult);

            // dependencies
            var deletePostResult = await _client.DeletePost(new()
            {
                Id = createPostId
            });

            Assert.True(deletePostResult);

            var deleteSpaceResult = await _client.DeleteSpace(new()
            {
                Id = createSpaceId
            });

            Assert.True(deleteSpaceResult);
        }

        [Fact]
        public async Task CustomQueryOrMutation_Pass()
        {
            // dependencies
            var members = await _client.GetMembers();
            Assert.NotEmpty(members);

            var firstMemberId = members.First().Id;

            var result1 = await _client.CustomQueryOrMutation<GetMemberResponse>("query request($id: ID!) { member(id: $id) { id }}", new { id = firstMemberId });
            Assert.NotNull(result1);

            var result2 = await _client.CustomQueryOrMutation<GetMemberResponse>($"query {{ member(id: \"{firstMemberId}\") {{ id }}}}");
            Assert.NotNull(result2);
        }

        [Fact]
        public void GetSingleSignOnJsonWebToken_Pass()
        {
            var token = _client.GetSingleSignOnJsonWebToken(_configuration.JwtSecretKey, new SingleSignOnUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "functionaltest@functiontest.com",
                Name = "Functional Test",
                TagLine = string.Empty
            });

            Assert.NotEmpty(token);
        }

        [Fact]
        public void OpenWebBrowserForSingleSignOnWithJsonWebToken_Pass()
        {
            _client.OpenWebBrowserForSingleSignOnWithJsonWebToken(_configuration.SingleSignOnUrl, _configuration.JwtSecretKey, new SingleSignOnUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "functionaltest@functiontest.com",
                Name = "Functional Test",
                TagLine = "Functional TagLine"
            });
        }
    }
}
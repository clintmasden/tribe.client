# tribe.client
A .NET Standard/C# implementation of Tribe.so API

---

| Name | Resources |
| ------ | ------ |
| Playground | https://app.tribe.so/graphql |
| Getting Started | https://partners.tribe.so/docs/guide/graphql/getting-started |
| JWT SSO | https://partners.tribe.so/docs/guide/single-sign-on/jwt-sso/ |
||
| GraphQL Formatter | https://jsonformatter.org/graphql-formatter

---

## Getting Started:
```
using System;
using Tribe.Client;

namespace Tribe.App
{
    public static class Program
    {
        private async Task Main(string[] args)
        {
            var client = new TribeClient("community.xxx.com");
            await client.SetAccessToken("usernameOrEmail", "password");

            var members = await client.GetMembers();

            members.ForEach(user => Console.WriteLine($"{user.Id}. {user.Name}"));
            Console.Read();
        }
    }
}
```

---

### Pull Requests + Additional Features + Releases

> Not all mutations (inputs) or queries are deployed.  ```Bad Requests``` and ```Internal Server Errors``` were experienced.  

> Provide a pull request for each mutation, query, or input change.  Each PR will be reviewed and deployed to the next release of the ```tribe.client```. 

#### Exposed (templated) Methods

| Methods | Notes |
| ------ | ------ |
| networkDomain | Ok |
| loginNetwork | Ok |
| members | Ok |
| member | Ok |
| joinNetwork | Ok & Error - Unable to pass all properties |
| updateMember | Unable to implement or verify & HTTP Bad Request or Internal Server Error |
| deleteMember | Ok |
| addSpaceMembers | Ok |
| removeSpaceMembers | Ok |
| tags | Ok |
| posts | Ok |
| post | Ok |
| feed | Ok |
| postTypes | Ok |
| createPost | Ok & Error - Unable to pass all properties |
| createReply | Ok & Error - Unable to pass all properties |
| updatePost | Unable to implement or verify & HTTP Bad Request or Internal Server Error |
| deletePost | Ok |
| collections | Ok |
| collection | Ok |
| createCollection | Ok |
| updateCollection | Unable to implement or verify & HTTP Bad Request or Internal Server Error |
| deleteCollection | Ok |
| spaces | Ok |
| space | Ok |
| createSpace | Ok & Error - Unable to pass all properties |
| updateSpace | Unable to implement or verify & HTTP Bad Request or Internal Server Error |
| deleteSpace | Ok

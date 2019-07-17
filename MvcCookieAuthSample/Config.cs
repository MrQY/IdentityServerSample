using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace MvcCookieAuthSample
{
    public class Config
    {
        public static List<ApiResource> GetResources()
        {
            return new List<ApiResource>{
                new ApiResource("api1","My Api")
            };
        }

        public static List<Client> GetClients()
        {
            return new List<Client> {
                new Client
                {
                    ClientId="mvc_client",
                    //implicit模式和授权码模式(authorization_code)访问差不多，相比之下，少了一步获取code的步骤，而是直接获取token
                    AllowedGrantTypes=GrantTypes.Implicit,
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    RedirectUris = {"http://localhost:5005/signin-oidc"}, //客户端的登录退出地址
                    PostLogoutRedirectUris = {"http://localhost:5005/signout-callback-oidc"},
                    RequireConsent=false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser
                {
                    SubjectId="1",
                    Username="admin",
                    Password="admin",
                    IsActive=true
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}

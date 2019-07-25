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
                    ClientName="MVC Client",
                    ClientUri="http://localhost:5005",
                    LogoUri="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1518157900003&di=40591ce365b775a870e10444d2fad18f&imgtype=0&src=http%3A%2F%2Fawb.img.xmtbang.com%2Fimg%2Fuploadnew%2F201507%2F06%2F3b5d45fde92d4042ae6d59ff05aded0f.jpg",

                    //implicit模式和授权码模式(authorization_code)访问差不多，相比之下，少了一步获取code的步骤，而是直接获取token
                    //AllowedGrantTypes=GrantTypes.Implicit,
                    AllowedGrantTypes=GrantTypes.Hybrid,
                    AllowOfflineAccess=false,//这是否可以指定客户请求刷新tokens
                    AllowAccessTokensViaBrowser=true,////指定此客户端是否允许通过浏览器接收访问令牌
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    RedirectUris = {"http://localhost:5005/signin-oidc"}, //客户端的登录退出地址
                    PostLogoutRedirectUris = {"http://localhost:5005/signout-callback-oidc"},
                    AlwaysIncludeUserClaimsInIdToken=true,
                    RequireConsent=true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        //IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.Email,
                        "api1"
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

using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerCenter
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
                    ClientId="client",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    AllowedScopes={"api1"}
                },
                new Client
                {
                    ClientId="pwd_client",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    AllowedScopes={"api1"}
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
    }
}

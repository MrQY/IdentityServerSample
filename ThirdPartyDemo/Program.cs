using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ThirdPartyDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
                throw new Exception(disco.Error);

            //ClientCredentialsTokenRequest tokenRequest = new ClientCredentialsTokenRequest
            //{
            //    Address=disco.TokenEndpoint,
            //    ClientId= "client",
            //    ClientSecret= "Secret",
            //    Scope= "api1"
            //};
            //TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest);

            PasswordTokenRequest tokenRequest = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "pwd_client",
                ClientSecret = "Secret",
                Scope = "api1",
                UserName="admin",
                Password="admin"
            };
            TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(tokenRequest);
            if (tokenResponse.IsError)
                Console.WriteLine(tokenResponse.Error);
            client.SetBearerToken(tokenResponse.AccessToken);
            string result = await client.GetStringAsync("http://localhost:5001/api/values");

            Console.WriteLine(result);

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}

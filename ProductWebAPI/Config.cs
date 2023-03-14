using IdentityModel;
using IdentityServer4.Models;
using static IdentityModel.OidcConstants;
using GrantTypes = IdentityServer4.Models.GrantTypes;

namespace ProductWebAPI
{
    public class Config
    {
        public static IEnumerable<ApiScope> ApiScoppes =>
            new ApiScope[]
            {
                new ApiScope("productwebapi")
            };
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
               new ApiResource("productwebapi", "product web api")
               {
                   Scopes = { "productwebapi" },
               }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {
                        new Secret("secret".ToSha256()),
                    },
                    AllowedScopes = { "productwebapi" }
                }
            };
        }
    }
}

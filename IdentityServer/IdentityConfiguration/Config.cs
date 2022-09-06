using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.IdentityConfiguration
{
    public static class Config
    {
        public static List<Client> Clients = new List<Client> // Register clients thar are allowed to use IdentityServer4.
        {
                new Client
                {
                    ClientId = "employeeManagementApi",
                    ClientName = "EmployeeManagement Api",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("jsf5_fs38@ql")},
                    AllowedScopes = new List<string> { "employeeManagementApi.read", "openid", "profile", "email" }
                }
        };

        public static List<ApiResource> ApiResources = new List<ApiResource> // The api that IdentityServer4 is protecting.
        {
                new ApiResource
                {
                    Name = "employeeManagementApi",
                    DisplayName = "EmployeeManagement Api",
                    Description = "Allow the application to access EmployeeManagement Api on your behalf",
                    Scopes = new List<string> { "employeeManagementApi.read", "employeeManagementApi.write" },
                    ApiSecrets = new List<Secret> {new Secret("jsf5_fs38@ql")},
                    UserClaims = new List<string> {"role"}
                }
        };

        public static IEnumerable<ApiScope> ApiScopes = new List<ApiScope> // What actions authorized users can perform at the level of the API.
        {
            new ApiScope("employeeManagementApi.read", "Read access to Employee Management Api"),
            new ApiScope("employeeManagementApi.write", "Write access to Employee Management Api")
        };
    }
}

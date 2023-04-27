using Identity.Model;
using IdentityModel;
using IdentityServer4.Models;

namespace Identity;

public class ApiResourcesScope
{
    public const string FileStorage = "fs";
    public const string Notifications = "ntf";
    public const string Contact = "ct";
    public const string PetInfo = "pi";
}

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("roles", new[] { "role" })
        };

    public static IEnumerable<ApiResource> GetApiResources(IConfigurationSection configurationSection)
    {
        var apiResources = new List<ApiResource>()
        {
            new ApiResource(ApiResourcesScope.FileStorage, "File Storage Service")
            {
                ApiSecrets = { new Secret("fs".ToSha256()) }
            },
            new ApiResource(ApiResourcesScope.Notifications, "Notifications Service")
            {
                ApiSecrets = { new Secret("ntf".ToSha256()) }
            },
            new ApiResource(ApiResourcesScope.Contact, "Contact Service")
            {
                ApiSecrets = { new Secret("ct".ToSha256()) }
            },
            new ApiResource(ApiResourcesScope.PetInfo, "Pet Info Service")
            {
                ApiSecrets = { new Secret("pi".ToSha256()) }
            }
        };

        var integrationApis = new List<IntegrationApi>();
        
        configurationSection.Bind(integrationApis);

        foreach (var integrationApi in integrationApis.Where(x => x.Active))
        {
            apiResources.Add(new ApiResource(integrationApi.Name, integrationApi.DisplayName)
            {
                ApiSecrets = { new Secret(integrationApi.Name.ToSha256()) }
            });
        }

        return apiResources;
    }
}
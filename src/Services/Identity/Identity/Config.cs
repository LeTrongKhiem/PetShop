using Identity.Model;
using IdentityModel;
using IdentityServer4;
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
            },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
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

    public static IEnumerable<Client> GetClients(IConfigurationSection config, IConfigurationSection apiResources,
        IConfigurationSection integrationApisConfig)
    {
        var integrationApis = new List<IntegrationApi>();
        integrationApisConfig.Bind(integrationApis);
        var integrationApiResources = integrationApis.Where(x => x.Active);
        var directorScopes = new List<string>
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            "roles",
            ApiResourcesScope.FileStorage,
            ApiResourcesScope.Notifications,
            ApiResourcesScope.Contact,
            ApiResourcesScope.PetInfo,
            IdentityServerConstants.LocalApi.ScopeName
        };
        var managerScopes = new List<string>
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            "roles",
            ApiResourcesScope.FileStorage,
            ApiResourcesScope.Notifications,
            ApiResourcesScope.Contact,
            ApiResourcesScope.PetInfo,
            IdentityServerConstants.LocalApi.ScopeName
        };
        var empScopes = new List<string>
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            "roles",
            ApiResourcesScope.FileStorage,
            ApiResourcesScope.Notifications,
            ApiResourcesScope.Contact,
            ApiResourcesScope.PetInfo,
            IdentityServerConstants.LocalApi.ScopeName
        };
        var cusScopes = new List<string>
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            "roles",
            ApiResourcesScope.FileStorage,
            ApiResourcesScope.Notifications,
            ApiResourcesScope.Contact,
            ApiResourcesScope.PetInfo,
            IdentityServerConstants.LocalApi.ScopeName
        };
        var guestScopes = new List<string>
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            "roles",
            ApiResourcesScope.FileStorage,
            ApiResourcesScope.Notifications,
            ApiResourcesScope.Contact,
            ApiResourcesScope.PetInfo,
            IdentityServerConstants.LocalApi.ScopeName
        };
        var enumerable = integrationApiResources.ToList();
        if (enumerable.Any())
        {
            directorScopes.AddRange(enumerable.Select(x => x.Name));
            managerScopes.AddRange(enumerable.Select(x => x.Name));
            empScopes.AddRange(enumerable.Select(x => x.Name));
            cusScopes.AddRange(enumerable.Select(x => x.Name));
            guestScopes.AddRange(enumerable.Select(x => x.Name));
        }

        var mvcClients = new List<Client>
        {
            new Client()
            {
                ClientId = "director",
                ClientName = "Director",
                ClientUri = config.GetValue<string>("Director:Url"),
                Enabled = config.GetValue<bool>("Director:Active"),
                ClientSecrets = new List<Secret>
                {
                    new Secret("directorsecret".ToSha256())
                },
                AllowedGrantTypes = new List<string>
                {
                    GrantType.Hybrid,
                    //   "refresh_token"
                },
                RequireConsent = false,
                // AllowOfflineAccess = true,
                //AccessTokenLifetime = 60 * 2,
                //IdentityTokenLifetime = 60 * 2,
                //RefreshTokenExpiration = TokenExpiration.Absolute,
                //AbsoluteRefreshTokenLifetime = 60 * 2,
                AllowAccessTokensViaBrowser = false,
                AlwaysIncludeUserClaimsInIdToken = false,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris = new List<string>
                {
                    config.GetValue<string>("Director:Url") + "/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    config.GetValue<string>("Director:Url") + "/signout-callback-oidc"
                },
                AllowedScopes = directorScopes,
            },
            new Client()
            {
                ClientId = "manager",
                ClientName = "Manager",
                ClientUri = config.GetValue<string>("Manager:Url"),
                Enabled = config.GetValue<bool>("Manager:Active"),
                ClientSecrets = new List<Secret>
                {
                    new Secret("managersecret".ToSha256())
                },
                AllowedGrantTypes = new List<string>
                {
                    GrantType.Hybrid,
                    //   "refresh_token"
                },
                RequireConsent = false,
                // AllowOfflineAccess = true,
                //AccessTokenLifetime = 60 * 2,
                //IdentityTokenLifetime = 60 * 2,
                //RefreshTokenExpiration = TokenExpiration.Absolute,
                //AbsoluteRefreshTokenLifetime = 60 * 2,
                AllowAccessTokensViaBrowser = false,
                AlwaysIncludeUserClaimsInIdToken = false,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris = new List<string>
                {
                    config.GetValue<string>("Manager:Url") + "/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    config.GetValue<string>("Manager:Url") + "/signout-callback-oidc"
                },
                AllowedScopes = managerScopes,
            },
            new Client()
            {
                ClientId = "employee",
                ClientName = "Employee",
                ClientUri = config.GetValue<string>("Employee:Url"),
                Enabled = config.GetValue<bool>("Employee:Active"),
                ClientSecrets = new List<Secret>
                {
                    new Secret("employeesecret".ToSha256())
                },
                AllowedGrantTypes = new List<string>
                {
                    GrantType.Hybrid,
                    //   "refresh_token"
                },
                RequireConsent = false,
                // AllowOfflineAccess = true,
                //AccessTokenLifetime = 60 * 2,
                //IdentityTokenLifetime = 60 * 2,
                //RefreshTokenExpiration = TokenExpiration.Absolute,
                //AbsoluteRefreshTokenLifetime = 60 * 2,
                AllowAccessTokensViaBrowser = false,
                AlwaysIncludeUserClaimsInIdToken = false,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris = new List<string>
                {
                    config.GetValue<string>("Employee:Url") + "/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    config.GetValue<string>("Employee:Url") + "/signout-callback-oidc"
                },
                AllowedScopes = empScopes,
            },
            new Client()
            {
                ClientId = "customer",
                ClientName = "Customer",
                ClientUri = config.GetValue<string>("Customer:Url"),
                Enabled = config.GetValue<bool>("Customer:Active"),
                ClientSecrets = new List<Secret>
                {
                    new Secret("customersecret".ToSha256())
                },
                AllowedGrantTypes = new List<string>
                {
                    GrantType.Hybrid,
                    //   "refresh_token"
                },
                RequireConsent = false,
                // AllowOfflineAccess = true,
                //AccessTokenLifetime = 60 * 2,
                //IdentityTokenLifetime = 60 * 2,
                //RefreshTokenExpiration = TokenExpiration.Absolute,
                //AbsoluteRefreshTokenLifetime = 60 * 2,
                AllowAccessTokensViaBrowser = false,
                AlwaysIncludeUserClaimsInIdToken = false,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris = new List<string>
                {
                    config.GetValue<string>("Customer:Url") + "/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    config.GetValue<string>("Customer:Url") + "/signout-callback-oidc"
                },
                AllowedScopes = cusScopes,
            },
            new Client()
            {
                ClientId = "guest",
                ClientName = "Guest",
                ClientUri = config.GetValue<string>("Guest:Url"),
                Enabled = config.GetValue<bool>("Guest:Active"),
                ClientSecrets = new List<Secret>
                {
                    new Secret("guestsecret".ToSha256())
                },
                AllowedGrantTypes = new List<string>
                {
                    GrantType.Hybrid,
                    //   "refresh_token"
                },
                RequireConsent = false,
                // AllowOfflineAccess = true,
                //AccessTokenLifetime = 60 * 2,
                //IdentityTokenLifetime = 60 * 2,
                //RefreshTokenExpiration = TokenExpiration.Absolute,
                //AbsoluteRefreshTokenLifetime = 60 * 2,
                AllowAccessTokensViaBrowser = false,
                AlwaysIncludeUserClaimsInIdToken = false,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris = new List<string>
                {
                    config.GetValue<string>("Guest:Url") + "/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    config.GetValue<string>("Guest:Url") + "/signout-callback-oidc"
                },
                AllowedScopes = guestScopes,
            }
        };
        var integrationClients = new List<Client>();

        #region Integration

        //this needs to be moved to integrationapis [] 
        var integrationApiUrl = config.GetValue<string>("IntegrationApi:Url");
        if (!string.IsNullOrWhiteSpace(integrationApiUrl))
        {
            integrationClients.Add(
                new Client
                {
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("integrationapi1".ToSha256()) },
                    AllowedScopes =
                    {
                        ApiResourcesScope.Notifications,
                        ApiResourcesScope.FileStorage,
                        ApiResourcesScope.Contact,
                        ApiResourcesScope.PetInfo,
                        IdentityServerConstants.LocalApi.ScopeName
                    },
                    Enabled = config.GetValue<bool>("IntegrationApi:Active"),
                    ClientId = "integrationapi",
                    ClientName = "Integration API",
                });
        }

        foreach (var integrationApi in integrationApis)
        {
            if (integrationApi.Swagger != default)
            {
                integrationClients.Add(
                    new Client
                    {
                        AllowAccessTokensViaBrowser = true,
                        AllowedGrantTypes = GrantTypes.Implicit,
                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.LocalApi.ScopeName,
                            integrationApi.Name,
                        },
                        ClientId = integrationApi.Swagger.ClientId,
                        RequireConsent = false,
                        ClientName = integrationApi.Swagger.ClientName,
                        RedirectUris = { integrationApi.Swagger.Url + "/swagger/oauth2-redirect.html" }
                    });
            }
        }

        integrationClients.Add(
            new Client
            {
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("twilioCallback1".ToSha256()) },
                AllowedScopes =
                {
                    ApiResourcesScope.Notifications,
                    ApiResourcesScope.Contact,
                    IdentityServerConstants.LocalApi.ScopeName
                },
                ClientId = "twilioCallback",
                ClientName = "Twilio Callback",
            });

        #endregion Integration

        mvcClients = mvcClients.Where(x => !string.IsNullOrWhiteSpace(x.ClientUri)).ToList();
        var clients = mvcClients
            // .Concat(webservices)
            // .Concat(swaggerDocs)
            .Concat(integrationClients);

        return clients;
    }
}
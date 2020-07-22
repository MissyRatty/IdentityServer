using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>() 
        { 
            new IdentityResources.OpenId(), //since this is set here as a resource that clients have access, we will have to add it to the list of allowed scopes: see line 59
            new IdentityResources.Profile(),
            new IdentityResource()           // this is a possible scope that could be requested and it will have these claims
            {
               Name = "my.OwnDefinedScope",
               //this claim will be added to the id_token
               UserClaims = 
                {
                   "my.Claim"
                }
            }
        };

        /// <summary>
        /// This provides a list of static api resources (scopes), being registered so Identity Server knows about them. 
        /// ApiResource is IdentityServer's way of identifying apis.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis() => new List<ApiResource>()
        {
            //if you want to add a claim to your access_token, then you have to add it to your API resource like: apiResource("APIName", ["the claim type names as a string"])
            new ApiResource("ServerApi"),
            new ApiResource("ClientApi", new string[] { "my.api.claim" })  //this was only added when we wanted the mvc client app to have access to both Apis

        };

        /// <summary>
        /// This provides a list of static clients that can communicate with the ServerApi.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients() => new List<Client>()
        {
            //this client was used for the Client Credentials Authorisation Flow
            //this can be used for system - system calls/communication
            new Client
            {
                ClientId = "clientApi_id",
                ClientSecrets =
                {
                    new Secret(value: "clientApi_secret".ToSha256()) //this can be set to expire if we wanted
                },
                AllowedGrantTypes = GrantTypes.ClientCredentials, //this is how we would be retrieving the access tokens
                AllowedScopes = { "ServerApi" }                  //this is the apis' that this client with id "client1_id", is allowed/can have access to.If empty, the client will have no access to any apis.
            },

            //It is used by both web apps and native/mobile apps to get an access token after a user authorizes an app.
            //This flow loads in a browser and takes the user to the OAuthServer, which will then prompt the user to either allow/deny the client app access to some info, 
            //once the user has allowed the client app, user is redirected to the client app with an authorisation code (in the querystring). However the life span of the auth code can be decided by the AuthService that issued it
            //the client app can now, using the authorisation code, to exchange that for an access token.
            //see: https://developer.okta.com/blog/2018/04/10/oauth-authorization-code-grant-type
             new Client
            {
                ClientId = "clientMvc_id",
                ClientSecrets =
                {
                    new Secret(value: "clientMvc_secret".ToSha256()) //this can be set to expire if we wanted
                },
                AllowedGrantTypes = GrantTypes.Code, //this is how we would be retrieving the access tokens

                //this is the apis' that this client with id "clientMvc_id", is allowed/can have access to.If empty, the client will have no access to any apis.
                AllowedScopes = 
                 { 
                     "ServerApi", 
                     "ClientApi", 
                     IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                     IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                     "my.OwnDefinedScope"
                 },  
                RedirectUris = { "https://localhost:44392/signin-oidc" },
                RequireConsent = false,                                    //disabling the user-consent screen.

                //puts all the claims in the id_token, this could grow big
                //AlwaysIncludeUserClaimsInIdToken = true


                //set this to true to be able to request for a refresh_token
                AllowOfflineAccess = true
            }
        };
    }
}

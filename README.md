# IdentityServer

1. https://localhost:44338/.well-known/openid-configuration 
   - Visit this endpoint to see all the auth endpoints provided by IdentityServer 4 e.g.: the token endpoint, authorization endpoint etc.
     These endpoints specify how applications can interact with your identity server implementation.


2. Angular Apps in this POC:

   - AngularClientApp Folder, has an angular app that uses the 'angular-auth-oidc-client' package to implement the PKCE flow with Identity Server 4

   - SPA_Test Folder, has an angular app that uses the 'oidc-client' package to implement the PKCE flow as well.
import { Injectable } from '@angular/core';

import { UserManager, UserManagerSettings, User, WebStorageStateStore } from 'oidc-client';

export function getClientConfigurations(): UserManagerSettings {
  return {
    authority: 'https://localhost:44338',
    client_id: 'angularApp_oidc_client_lib',
    redirect_uri: 'http://localhost:4200/auth-callback',
    post_logout_redirect_uri: 'http://localhost:4200/logout',
    response_type: 'code',
    scope: 'openid profile ServerApi',
    filterProtocolClaims: true,
    loadUserInfo: true,
    //response_mode: 'query',
    userStore: new WebStorageStateStore({ store: window.localStorage })
  }
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userManager = new UserManager(getClientConfigurations());
  private appUser: User = null;

  constructor() {
    //this gets the current authenticated user
    this.userManager.getUser().then(user => {
      this.appUser = user;
    });
   }

   // check if we have a user and if the said user is still valid (i.e.: has the user's access token expired ?)
   isUserLoggedIn(): boolean {
     return this.appUser != null && !this.appUser.expired;
   }

   //get user claims from the user profile, since filterProtocolClaims is set to true in the configuration, 
   //protocal level claims e.g.: nbf, iss, at_hash, nonce won't be extracted from the id_token as profile data
   getUserClaims(): any {
     return this.appUser.profile;
   }

   //gets the auth header value e.g.: Authorization: Bearer TokeValue, from the user object
   getAuthorizationHeaderValue() {
    return `${this.appUser.token_type} ${this.appUser.access_token}`;
   }

   //The methods below do the heavy lifting of our angular app interacting with the identity provider
   //Handles OpenID Connect auth requests for us, using 'signinRedirect' and 'siginRedirectCallback' methods from the oidc-client package
   // 'signinRedirect' and 'siginRedirectCallback' methods will automatically redirect users to our ID4 server using our user manager configurations
   //our user manager configurations are the results from our getClientConfigurations method on line 5

   //An alternative to this would be to use signinPopup and signinPopupCallback which will open a new window for the request instead of a redirection.

   startUserAuthentication(): Promise<void> {
    //signInRedirect: this creates the authorization request to our ID4 Server, handles state, nonce, and could call the metadata endpoint as well if required
     return this.userManager.signinRedirect();
   }

  async completeUserAuthentication(): Promise<void> {
     //callback func: receives and handles incoming tokens (incl: token validation).
     //if the usermanagersettings provided on line 5 includes the 'loadUserInfo = true', 
     //then the callback will also call the userInfo endpoint and get any extra info it has been authorized to access
     //the callback method returns a promise of the authenticated user.
     const user = await this.userManager.signinRedirectCallback();
     this.appUser = user;
   }

   refreshToken(): Promise<User> {
     return this.userManager.signinSilent();
   }
}

import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AuthModule, LogLevel, OidcConfigService } from 'angular-auth-oidc-client';
import { HttpClientModule } from '@angular/common/http';

export function configureAuth(oidcConfigService: OidcConfigService) {
  return () =>
      oidcConfigService.withConfig({
        clientId: 'angularApp_authOidc_lib',
        storage: window.localStorage,           // persist token in local storage, by default, this is stored in session storage
        stsServer: 'https://localhost:44338',  //this is the identity server address
        responseType: 'code',
        redirectUrl: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
        scope: 'openid ServerApi',
        //silentRenew: true,
        // silentRenewUrl: `${window.location.origin}/silent-renew.html`,
        logLevel: LogLevel.Debug,    //in prod, don't forget to change this log level
      });
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule.forRoot(),
    HttpClientModule,
  ],
  providers: [
    OidcConfigService, 
    {
      provide: APP_INITIALIZER,
      useFactory: configureAuth,
      deps: [ OidcConfigService ],
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

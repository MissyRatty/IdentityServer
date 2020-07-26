import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';

import { AuthGuardService } from './services/auth-guard.service';
import { AuthService } from './services/auth.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { GetApiDataService } from './services/get-api-data.service';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { LogoutComponent } from './logout/logout.component';
import { GetDataFromApiComponent } from './get-data-from-api/get-data-from-api.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthCallbackComponent,
    LogoutComponent,
    GetDataFromApiComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [ AuthGuardService, AuthService, GetApiDataService ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }

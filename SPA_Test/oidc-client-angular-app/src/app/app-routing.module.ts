import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from './services/auth-guard.service';
import { HomeComponent } from './home/home.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { LogoutComponent } from './logout/logout.component';
import { GetDataFromApiComponent } from './get-data-from-api/get-data-from-api.component';

const routes: Routes = [
  {
    path: '',
    children: []
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate:[ AuthGuardService ]  // add this to restrict access to the home page until a user has been successfully authenticated
  },
  {
    path: 'auth-callback',
    component: AuthCallbackComponent
  },
  {
    path: 'get-data-from-api',
    component: GetDataFromApiComponent,
    canActivate: [ AuthGuardService ]   // add this to restrict access to the get data from api page to only authenticated users
  },
  {
    path: 'logout',
    component: LogoutComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

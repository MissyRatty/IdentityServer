import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthService) { }

  canActivate(): boolean {

    //check if user has been authenticated
    if(this.authService.isUserLoggedIn()) {
      return true;
    }

    //if user is not logged in, start authentication
    this.authService.startUserAuthentication();
    return false;
  }
}

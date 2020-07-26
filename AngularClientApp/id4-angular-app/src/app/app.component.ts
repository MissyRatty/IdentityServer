import { Component } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  //title = 'id4-angular-app';

  disableCallApiButton = true;
  authenticationSucceeded = false;
  resultsFromApi = '';
  showApiResults = false;

  constructor(public oidcSecurityService: OidcSecurityService, public httpClient: HttpClient) {}

  ngOnInit() {
    this.oidcSecurityService
      .checkAuth()
      .subscribe((successfulAuthentication) => {
        console.log('is authenticated', successfulAuthentication);
        this.authenticationSucceeded = successfulAuthentication;

        console.log('authenticationSucceeded', this.authenticationSucceeded);
      });
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  //using our access_token to call our api endpoint
  callApiEndpoint() {
    
    //server api endpoint
    const apiEndpointUrl = "https://localhost:44333/data";

    //get access_token
    const token = this.oidcSecurityService.getToken();

    //set up the http headers
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + token,
      }),
      responseType: 'text' as 'text',
    };

    //make the http get request
    //this returns an observable so we subscribe to the results
    this.httpClient.get(apiEndpointUrl, httpOptions)
      .subscribe((data: any) => { 
        this.resultsFromApi = data;
        this.showApiResults = true;
        console.log('api results => ', data);
      });
  }

  logout() {
    this.oidcSecurityService.logoff();
  }
}

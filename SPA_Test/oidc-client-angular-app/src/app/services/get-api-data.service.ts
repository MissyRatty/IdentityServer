import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GetApiDataService {

  constructor(private httpClient: HttpClient, private authService: AuthService) {}

  getDataFromApi(apiEndpointUrl: string): Observable<string> {

    //this will return  'Bearer access_tokenValue'
    const authorizationHeader = this.authService.getAuthorizationHeaderValue();
    let requestHeaders = new HttpHeaders({ 'Authorization': authorizationHeader });

    return this.httpClient.get(apiEndpointUrl, { headers: requestHeaders, responseType: 'text' as 'text' });
  }
}

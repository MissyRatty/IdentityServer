import { Component, OnInit } from '@angular/core';

import { GetApiDataService } from '../services/get-api-data.service';

@Component({
  selector: 'app-get-data-from-api',
  templateUrl: './get-data-from-api.component.html',
  styleUrls: ['./get-data-from-api.component.css']
})
export class GetDataFromApiComponent implements OnInit {

  responseFromApi: string;

  constructor(private getApiDataService: GetApiDataService) {}

  ngOnInit() {
    const apiEndpoint = "https://localhost:44333/data";
    this.getApiDataService.getDataFromApi(apiEndpoint).subscribe(
      response => { 
        this.responseFromApi = response;
        console.log('compo local var =>', this.responseFromApi);
    }, 
    error => {
      this.responseFromApi = 'An error occurred, check console window';
      console.log(error);
    });
  }
}

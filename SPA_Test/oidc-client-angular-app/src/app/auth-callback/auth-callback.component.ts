import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  constructor(private authService: AuthService) { }

  async ngOnInit() {
    //receives and handles incoming tokens from the ID4 Server
    await this.authService.completeUserAuthentication();
  }

}

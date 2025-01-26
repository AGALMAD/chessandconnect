import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/dto/user';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {

    constructor(
      private authService: AuthService
    ) {
    }

    User(){
      return this.authService.getUser()
    }

    usuarioToken() {
      return this.authService.loged() 
    }

    closeSession(){
      this.authService.logout()
    }
}

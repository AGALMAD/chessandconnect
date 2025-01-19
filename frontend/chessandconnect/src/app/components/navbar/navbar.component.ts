import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

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

    usuarioToken() {
      return this.authService.loged() 
    }

    closeSession(){
      this.authService.logout()
    }
}

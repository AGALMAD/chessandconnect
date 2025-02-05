import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { RouterLink } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {

  public baseUrl = environment.apiUrl; 

  constructor(
    public authService: AuthService
  ) {
  }

  User() {
    return this.authService.getUser()
  }

  usuarioToken() {
    return this.authService.loged()
  }

  closeSession() {
    this.authService.logout()
  }
}

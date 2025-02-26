import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { User } from '../../models/dto/user';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-user-profile',
  imports: [NavbarComponent, CommonModule],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
  activeTab: string = 'chess';
  public baseUrl = environment.apiUrl; 
  
  constructor(
      public authService: AuthService
  ) {}

  cambiarTab(tab: string) {
    this.activeTab = tab;
  }


}

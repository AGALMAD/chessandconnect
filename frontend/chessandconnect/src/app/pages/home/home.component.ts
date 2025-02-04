import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { ApiService } from '../../services/api.service';
import { WebsocketService } from '../../services/websocket.service';
import { Subscription } from 'rxjs';
import { UserService } from '../../services/user.service';
import { RouterLink } from '@angular/router';
import { MenuService } from '../../services/menu.service';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-home',
  imports: [NavbarComponent,RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{

  constructor(private authService :AuthService,private api: ApiService){}

  ngOnInit(): void {
    if(this.api.jwt)
      this.authService.getCurrentUser()
  }

  



  

}

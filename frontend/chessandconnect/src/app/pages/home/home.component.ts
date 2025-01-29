import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { ApiService } from '../../services/api.service';
import { WebsocketService } from '../../services/websocket.service';
import { Subscription } from 'rxjs';
import { UserService } from '../../services/user.service';


@Component({
  selector: 'app-home',
  imports: [NavbarComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, OnDestroy{


  constructor(
    private api : ApiService,
    private webSocketService: WebsocketService,
    private userService: UserService
  ){}

  
  async ngOnInit(): Promise<void> {
    if(this.api.jwt){
      await this.webSocketService.connectRxjs()
    }
    if(!this.userService.currentUser){
      await this.userService.getUser()
    }
  }

  async ngOnDestroy(): Promise<void> {
    if(this.api.jwt){
      await this.webSocketService.disconnectRxjs()
      this.userService.currentUser = null
    }
  }
  
  

}

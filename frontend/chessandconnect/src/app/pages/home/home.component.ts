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
export class HomeComponent implements OnInit{


  constructor(
    private api : ApiService,
    private webSocketService: WebsocketService,
    private userService: UserService
  ){}

  async ngOnInit(): Promise<void> {
    await this.webSocketService.connectRxjs()
  }



  

}

import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { ApiService } from '../../services/api.service';
import { WebsocketService } from '../../services/websocket.service';


@Component({
  selector: 'app-home',
  imports: [NavbarComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  constructor(
    private api : ApiService,
    private websocketService: WebsocketService
  ){}

  ngOnInit(): void {
    console.log("JWT : " + this.api.jwt )
    this.websocketService.connectNative()
  }

}

import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { WebsocketService } from './services/websocket.service';
import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'chessandconnect';

  constructor() { }





}

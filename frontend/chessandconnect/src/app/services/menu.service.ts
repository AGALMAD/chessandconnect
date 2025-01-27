import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { WebsocketService } from './websocket.service';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  connected$: Subscription;
  isConnected: boolean = false;
  messageReceived$: Subscription;
  disconnected$: Subscription;

  usersLogged = 0;
  gamesInProgress = 0;



  constructor(public webSocketService: WebsocketService) {

    console.log('Mensaje recibido:');

    this.connected$ = this.webSocketService.connected.subscribe(() => this.isConnected = true);

    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(message => 
      console.log('Mensaje recibido: ' + message)
    );

    this.disconnected$ = this.webSocketService.disconnected.subscribe(() => this.isConnected = false);
  }


  private readMessage(message: string): void {
    console.log('Mensaje recibido:', message);
  }



}



import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { WebsocketService } from './websocket.service';

@Injectable({
  providedIn: 'root'
})
export class MenuService implements OnInit,OnDestroy{

  connected$: Subscription;
  isConnected: boolean = false;
  messageReceived$: Subscription;
  disconnected$: Subscription;
  
  usersLogged = 0;
  gamesInProgress = 0;


  
  constructor(private webSocketService: WebsocketService) { }


  ngOnInit(): void {
    this.connected$ = this.webSocketService.connected.subscribe(() => this.isConnected = true);

    this.messageReceived$ = this.webSocketService.messageReceived.subscribe((message: string) => {
      this.readMessage(message); 
    });

    this.disconnected$ = this.webSocketService.disconnected.subscribe(() => this.isConnected = false);

  }

  private readMessage(message: string): void {
    console.log('Mensaje recibido:', message); 
  }


  ngOnDestroy(): void {
    this.connected$.unsubscribe();
    this.messageReceived$.unsubscribe();
    this.disconnected$.unsubscribe();
  }


}


  
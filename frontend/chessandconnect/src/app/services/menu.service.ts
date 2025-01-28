import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { WebsocketService } from './websocket.service';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { ConnectionModel } from '../models/WebSocketMessages/ConnectionModel';

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

    this.connected$ = this.webSocketService.connected.subscribe(() => this.isConnected = true);

    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(message => 
      this.readMessage(message)
    );

    this.disconnected$ = this.webSocketService.disconnected.subscribe(() => this.isConnected = false);
  }


  private readMessage(message: string): void {
    console.log('Mensaje recibido:', message);

    try {
      // Paso del mensaje a objeto
      const parsedMessage = JSON.parse(message);

      const socketMessage = new SocketMessageGeneric<any>();
      socketMessage.Type = parsedMessage.Type as SocketCommunicationType;
      socketMessage.Data = parsedMessage.Data as ConnectionModel;


      this.handleSocketMessage(socketMessage);
    } catch (error) {
      console.error('Error al parsear el mensaje recibido:', error);
    }
  }

  private handleSocketMessage(message: SocketMessageGeneric<any>): void {
    switch (message.Type) {
      case SocketCommunicationType.CONNECTION:
        console.log('Mensaje de conexión recibido:', message.Data);
        this.usersLogged = message.Data.UsersCounter
        break;
    }
  }



}



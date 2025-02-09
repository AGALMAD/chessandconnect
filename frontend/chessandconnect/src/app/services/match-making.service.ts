import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { Friend } from '../models/dto/friend';
import { Subscription } from 'rxjs';
import { ApiService } from './api.service';
import { Router } from '@angular/router';
import { WebsocketService } from './websocket.service';
import { AuthService } from './auth.service';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { Room } from '../models/room';

@Injectable({
  providedIn: 'root'
})
export class MatchMakingService {

  isHost = false
  opponent: Friend

  messageReceived$: Subscription;


  constructor(
    private api: ApiService,
    private router: Router,
    public webSocketService: WebsocketService,
    public matchMakingService: MatchMakingService,
    public authService: AuthService) {

    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message =>
      await this.readMessage(message)
    );

  }


  private async readMessage(message: string): Promise<void> {
    console.log('Noe te quiere:', message);

    try {
      // Paso del mensaje a objeto
      const parsedMessage = JSON.parse(message);

      const socketMessage = new SocketMessageGeneric<any>();
      socketMessage.Type = parsedMessage.Type as SocketCommunicationType;
      socketMessage.Data = parsedMessage.Data;


      this.handleSocketMessage(socketMessage);
    } catch (error) {
      console.error('Error al parsear el mensaje recibido:', error);
    }
  }


  private async handleSocketMessage(message: SocketMessageGeneric<any>): Promise<void> {
    switch (message.Type) {
      case SocketCommunicationType.GAME_START:

        const newRoom = message.Data as Room;

        const opponentId = newRoom.Player1Id != this.authService.currentUser.id ? newRoom.Player1Id : newRoom.Player2Id



        break

    }

  }



}

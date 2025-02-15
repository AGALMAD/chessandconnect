import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { ChessPiece } from '../models/Games/Chess/ChessPiece';
import { WebsocketService } from './websocket.service';
import { Subscription } from 'rxjs';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  messageReceived$: Subscription;


  isHost = false
  opponent: User
  pieces: ChessPiece[]

  constructor(
    public webSocketService: WebsocketService,

  ) {
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

    console.log("BOARD:", message)

    switch (message.Type) {
      case SocketCommunicationType.CHESS_BOARD:

        const pieces = message.Data as ChessPiece[];
        this.pieces = pieces

        console.log("Pieces", pieces)

        break

    }

  }
}

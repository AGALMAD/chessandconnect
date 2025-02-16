import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { ChessPiece } from '../models/Games/Chess/ChessPiece';
import { WebsocketService } from './websocket.service';
import { Subscription } from 'rxjs';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { ChessPieceColor } from '../models/Games/Chess/Enums/Color';
import { ChessPieceMovements } from '../models/Games/Chess/ChessPiecesMovements';
import { ChessBoard } from '../models/Games/Chess/ChessBoard';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  messageReceived$: Subscription;


  playerColor: ChessPieceColor
  opponent: User

  pieces: ChessPiece[]

  player1Time: number
  player2Time: number

  turn : ChessPieceColor


  movements: ChessPieceMovements[]


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

        const board = message.Data as ChessBoard;
        console.log("Board", board)

        
        this.pieces = board.Pieces
        this.turn = board.Turn
        this.player1Time = board.Player1Time
        this.player2Time = board.Player2Time


        break

      case SocketCommunicationType.CHESS_MOVEMENTS:
        const movements = message.Data as ChessPieceMovements[];

        this.movements = movements

        console.log("Movements", movements)

        break

    }

  }
}

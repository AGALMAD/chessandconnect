import { Injectable } from '@angular/core';
import { WebsocketService } from './websocket.service';
import { AuthService } from './auth.service';
import { MatDialog } from '@angular/material/dialog';
import { GameService } from './game.service';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { ChessBoard } from '../models/Games/Chess/chess-board';
import { Subscription } from 'rxjs';
import { ChessPiece } from '../models/Games/Chess/chess-piece';
import { ChessPieceMovements } from '../models/Games/Chess/chess-pieces-movements';
import { PieceColor } from '../models/Games/Chess/Enums/Color';

@Injectable({
  providedIn: 'root'
})
export class ChessService {
  
  messageReceived$: Subscription;


  pieces: ChessPiece[]
  movements: ChessPieceMovements[]
  showMovements: ChessPieceMovements


  constructor(
    public webSocketService: WebsocketService,
    private authService: AuthService,
    private gameService: GameService,
    private dialog: MatDialog,
  ) {
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message =>
      await this.readMessage(message)
    );

  }

  private async readMessage(message: string): Promise<void> {
    console.log('Masage:', message);

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
        this.gameService.turn = board.Turn
        this.gameService.currentPlayerTimer = this.gameService.playerColor == PieceColor.WHITE ? board.Player1Time : board.Player2Time
        this.gameService.opponentTimer = this.gameService.playerColor == PieceColor.WHITE ? board.Player2Time : board.Player1Time

        this.showMovements = null
        this.gameService.startCountdown();

        break

      case SocketCommunicationType.CHESS_MOVEMENTS:
        const movements = message.Data as ChessPieceMovements[];

        this.movements = movements

        console.log("Movements", movements)

        break

    }


  }

}
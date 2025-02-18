import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { ChessPiece } from '../models/Games/Chess/ChessPiece';
import { WebsocketService } from './websocket.service';
import { interval, Subscription } from 'rxjs';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { ChessPieceColor } from '../models/Games/Chess/Enums/Color';
import { ChessPieceMovements } from '../models/Games/Chess/ChessPiecesMovements';
import { ChessBoard } from '../models/Games/Chess/ChessBoard';
import { AuthService } from './auth.service';
import { MatDialog } from '@angular/material/dialog';
import { ChessResultComponent } from '../components/chess-result/chess-result.component';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  messageReceived$: Subscription;
  private timerSubscription: any;

  opponent: User

  currentPlayerTimer: number
  opponentTimer: number

  turn: ChessPieceColor
  playerColor: ChessPieceColor


  winner: User = null
  looser: User = null


  constructor(
    public webSocketService: WebsocketService,
    private authService: AuthService,
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
      case SocketCommunicationType.END_GAME:
        const winnerId = message.Data;
        console.log("Winner", winnerId)

        this.winner = winnerId == this.authService.currentUser.id ? this.authService.currentUser : this.opponent
        this.looser = winnerId == this.authService.currentUser.id ? this.opponent : this.authService.currentUser

        this.dialog.open(ChessResultComponent, {
          width: '800px',
          data: {
            winner: this.winner,
            looser: this.looser
          }
        });

        break

    }


  }

  public startCountdown() {

    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe(); // Detener el anterior
    }

    this.timerSubscription = interval(1000).subscribe(() => {
      if (this.turn === ChessPieceColor.WHITE && this.playerColor == ChessPieceColor.WHITE) {
        this.currentPlayerTimer = Math.max(0, this.currentPlayerTimer - 1);
      } else {
        this.opponentTimer = Math.max(0, this.opponentTimer - 1);
      }

      console.log(this.currentPlayerTimer, this.opponentTimer)
    });
  }

}
import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { WebsocketService } from './websocket.service';
import { interval, Subscription } from 'rxjs';
import { SocketMessage, SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { AuthService } from './auth.service';
import { MatDialog } from '@angular/material/dialog';
import { ChessResultComponent } from '../components/chess-result/chess-result.component';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  messageReceived$: Subscription;
  private timerSubscription: any;

  opponent: User

  currentPlayerTimer: number
  opponentTimer: number

  turn: boolean = true
  playerColor: boolean


  winner: User = null
  looser: User = null


  constructor(
    public webSocketService: WebsocketService,
    private authService: AuthService,
    public dialog: MatDialog,
    private router: Router
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
      if (this.turn && this.playerColor) {
        this.currentPlayerTimer = Math.max(0, this.currentPlayerTimer - 1);
      } else {
        this.opponentTimer = Math.max(0, this.opponentTimer - 1);
      }

      console.log(this.currentPlayerTimer, this.opponentTimer)
    });
  }


  drawRequest(){

    const message : SocketMessage = {
      Type : SocketCommunicationType.DRAW_REQUEST,
    }
  }


  backToMenu() {
    this.router.navigate(['/menus']);
  }


  rematchRequest() {
    const message: SocketMessage = {
      Type: SocketCommunicationType.REMATCH_REQUEST,
    };

    this.webSocketService.sendRxjs(JSON.stringify(message));
  }

  offerDraw(){
    const message: SocketMessage = {
      Type: SocketCommunicationType.DRAW_REQUEST,
    };

    this.webSocketService.sendRxjs(JSON.stringify(message));
  }

  leaveGame(){
    const message: SocketMessage = {
      Type: SocketCommunicationType.LEAVE_GAME,
    };

    this.webSocketService.sendRxjs(JSON.stringify(message));
  }
}
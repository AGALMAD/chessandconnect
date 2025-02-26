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
import Swal from 'sweetalert2';

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

      case SocketCommunicationType.DRAW_REQUEST:
        Swal.fire({
          title: '<i class="fa-solid fa-chess-board"></i> ¡Solicitud de Tablas!',
          text: `${this.opponent?.userName ?? "Tu oponente"} propone tablas.`,
          toast: true,
          position: 'top-end',
          showConfirmButton: true,
          showCancelButton: true,
          confirmButtonText: 'Aceptar',
          cancelButtonText: 'Rechazar',
          timer: 10000,
          timerProgressBar: true,
          background: '#301e16',
          color: '#E8D5B5',
          customClass: {
            popup: 'rounded-lg shadow-lg',
            title: 'font-bold text-lg',
            confirmButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg',
            cancelButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg',
            timerProgressBar: 'bg-[#E8D5B5]'
          }
        }).then((result) => {
          if (result.isConfirmed) {
            this.drawRequest();
          }
        });

        break;


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



  backToMenu() {
    this.router.navigate(['/menus']);
  }


  rematchRequest() {
    const message: SocketMessage = {
      Type: SocketCommunicationType.REMATCH_REQUEST,
    };

    this.webSocketService.sendRxjs(JSON.stringify(message));
  }

  drawRequest() {
    const message: SocketMessage = {
      Type: SocketCommunicationType.DRAW_REQUEST,
    };

    this.webSocketService.sendRxjs(JSON.stringify(message));
  }

  leaveGame() {
    const message: SocketMessage = {
      Type: SocketCommunicationType.LEAVE_GAME,
    };

    this.webSocketService.sendRxjs(JSON.stringify(message));
  }



}
import { Injectable } from '@angular/core';
import { WebsocketService } from './websocket.service';
import { AuthService } from './auth.service';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { GameService } from './game.service';

import { ConnectBoard } from '../models/Games/Connect/connect-board';
import { ConnectPiece } from '../models/Games/Connect/connect-piece';
import { Router } from '@angular/router';
import { Room } from '../models/Games/room';
import { GameType } from '../enums/game';



@Injectable({
  providedIn: 'root'
})
export class ConnectService {

  messageReceived$: Subscription;


  pieces: ConnectPiece[] = []

  constructor(
    public webSocketService: WebsocketService,
    private authService: AuthService,
    private dialog: MatDialog,
    public gameService: GameService,
    private router: Router
  ) {

    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message =>
      await this.readMessage(message)
    );

  }


  private async readMessage(message: string): Promise<void> {

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
      case SocketCommunicationType.CONNECT_BOARD:

        const board = message.Data as ConnectBoard;
        console.log("CONNECT BOARD", board)

        if (board.LastPiece != null) {
          this.pieces.push(board.LastPiece)
          console.log("CONNECT PIECES", this.pieces)

        }

        this.gameService.turn = board.Player1Turn
        this.gameService.currentPlayerTimer = this.gameService.playerColor ? board.Player1Time : board.Player2Time
        this.gameService.opponentTimer = this.gameService.playerColor ? board.Player2Time : board.Player1Time

        this.gameService.startCountdown();


        break

      case SocketCommunicationType.GAME_START:

        const newRoom = message.Data as Room;
        if (newRoom.GameType == GameType.Connect4) {

          this.pieces = []

          this.gameService.currentPlayerTimer = 180
          this.gameService.opponentTimer = 180

          this.gameService.startCountdown()

          this.gameService.dialog.closeAll()

        }

        break

    }


  }
}
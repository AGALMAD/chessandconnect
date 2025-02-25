import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { WebsocketService } from '../../services/websocket.service';
import { GameService } from '../../services/game.service';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { ConnectService } from '../../services/connect.service';
import { SocketMessageGeneric } from '../../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../../enums/SocketCommunicationType';
import { CommonModule } from '@angular/common';
import { ChatComponent } from '../../components/chat/chat.component';
import { PipeTimerPipe } from "../../pipes/pipe-timer.pipe";

import { ConnectDropPieceRequest } from '../../models/Games/Connect/connect-drop-piece-request';


@Component({
  selector: 'app-connect4',
  imports: [CommonModule, ChatComponent, PipeTimerPipe],
  templateUrl: './connect4.component.html',
  styleUrl: './connect4.component.css'
})
export class Connect4Component {
  public baseUrl = environment.apiUrl;

  rows: number[] = [0, 1, 2, 3, 4, 5];  // 6 filas
  columns: number[] = [0, 1, 2, 3, 4, 5, 6];  // 7 columnas

  
  constructor(
    private websocketService:WebsocketService, 
    public gameService: GameService, 
    private api: ApiService, 
    public authService : AuthService,
    public connectService: ConnectService
  ) { }


  dropPiece(col: number){

    if(this.gameService.turn != this.gameService.playerColor)
      return

    const moveRequest: ConnectDropPieceRequest = { Column: col};

    const message : SocketMessageGeneric<ConnectDropPieceRequest> = {
      Type : SocketCommunicationType.CONNECT4_MOVEMENTS,
      Data : moveRequest
    }

    this.websocketService.sendRxjs(JSON.stringify(message))
  }

  
}

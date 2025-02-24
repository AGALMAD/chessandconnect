import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { WebsocketService } from '../../services/websocket.service';
import { GameService } from '../../services/game.service';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { ConnectService } from '../../services/connect.service';
import { SocketMessageGeneric } from '../../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../../enums/SocketCommunicationType';

import { ConnectDropPieceRequest } from '../../models/Games/Connect/connect-drop-piece-request';


@Component({
  selector: 'app-connect4',
  imports: [],
  templateUrl: './connect4.component.html',
  styleUrl: './connect4.component.css'
})
export class Connect4Component {
  public baseUrl = environment.apiUrl;


  
  constructor(
    private websocketService:WebsocketService, 
    public gameService: GameService, 
    private api: ApiService, 
    public authService : AuthService,
    public chessService: ConnectService
  ) { }


  dropPiece(col: number){
    const moveRequest: ConnectDropPieceRequest = { Column: col};

    const message : SocketMessageGeneric<ConnectDropPieceRequest> = {
      Type : SocketCommunicationType.CONNECT4_MOVEMENTS,
      Data : moveRequest
    }

    this.websocketService.sendRxjs(JSON.stringify(message))
  }

  
}

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
import { GameType } from '../enums/game';
import { GameService } from './game.service';
import { Room } from '../models/Games/room';
import { RoomRequest } from '../models/Games/room-request';


@Injectable({
  providedIn: 'root'
})
export class MatchMakingService {

  isHost = false
  friendOpponent: Friend

  messageReceived$: Subscription;


  constructor(
    private api: ApiService,
    private router: Router,
    public webSocketService: WebsocketService,
    public authService: AuthService,
    private gameService: GameService
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

    console.log("GAME:", message)
    switch (message.Type) {
      case SocketCommunicationType.GAME_START:

        const newRoom = message.Data as Room;

        console.log("ROOOOOM", newRoom)

        //Oponente
        const opponentId = newRoom.Player1Id != this.authService.currentUser.id ? newRoom.Player1Id : newRoom.Player2Id
        if(opponentId != 0){
          const result = await this.api.get<User>(`User/getUserById?id=${opponentId}`)
          this.gameService.opponent = result.data

        }
        else{
          this.gameService.opponent = {
            userName : "Magnus",
            avatarImageUrl: "UserProfilePicture/bot.png",
          }
        }

        
        if(newRoom.GameType == GameType.Chess)
        {
          this.gameService.playerColor = newRoom.Player1Id == this.authService.currentUser.id 
          this.router.navigate(
           ['/chessGame'],
          );
        }
        else if (newRoom.GameType == GameType.Connect4){

          this.gameService.playerColor = newRoom.Player1Id == this.authService.currentUser.id 
          this.router.navigate(
            ['/connectGame'],
          );
        }

        break

    }

  }

  async startGameWithFriend(gamemode: GameType){
      const room : RoomRequest = {
        Player2Id: this.friendOpponent.id,
        GameType: gamemode
      }
      await this.api.post<RoomRequest>(`MatchMaking/FriendGame`, room)
  }



}

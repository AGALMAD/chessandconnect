import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Router } from '@angular/router';
import { Friend } from '../models/dto/friend';
import { Result } from '../models/result';
import Swal from 'sweetalert2';
import { Friendship } from '../models/dto/friendship';
import { Subscription } from 'rxjs';
import { WebsocketService } from './websocket.service';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { ConnectionModel } from '../models/WebSocketMessages/ConnectionModel';
import { ConnectionType } from '../enums/ConnectionType';
import { GameInvitationModel } from '../models/WebSocketMessages/GameInvitationModel';
import { User } from '../models/dto/user';
import { FriendshipState } from '../enums/FriendshipState';


@Injectable({
  providedIn: 'root'
})
export class FriendsService {

  public connectedFriends: Friend[]
  public disconnectedFriends: Friend[]

  public gameInvitations: GameInvitationModel[]



  constructor(
    private api: ApiService,
    private router: Router,
    public webSocketService: WebsocketService
  ) {
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message =>
      await this.readMessage(message)
    );

    this.gameInvitations = [];

  }


  async getUsersByNickname(userNickname: String): Promise<Result<Friend>> {
    const result = await this.api.get<Friend>('Friendship/getusers', userNickname)
    if (!result.success) {
      this.handleError('Usuario no encontrado');
    }
    return result
  }

  async getFriends(query: string): Promise<void> {

    try {
      const result = await this.api.get<Friend[]>(`User/friends?query=${query}`);

      if (!result.success || !result.data) {
        this.handleError('No se encontraron amigos');
        return;
      }

      const allFriends = result.data;

      this.connectedFriends = allFriends.filter(friend => friend.connected);
      this.disconnectedFriends = allFriends.filter(friend => !friend.connected);

    } catch (error) {
      this.handleError('Error al obtener amigos');
      console.error(error);
    }

  }

  async acceptFriendshipRequest(id: number): Promise<Result<Friend>> {
    const result = await this.api.post<Friend>(`Friendship/acceptrequest?friendId=${id}`)
    if (result.success) {
      this.handleError('No se pudo aceptar la petición')
    }
    return result
  }

  async getAllFriendshipRequest(): Promise<Result<Friendship>> {
    const result = await this.api.get<Friendship>('friendship/getallrequests')
    if (result.success) {
      this.handleError('No se encontraron amigos')
    }
    return result
  }


  private handleError(message: string): void {
    Swal.fire({
      icon: 'error',
      text: message,
      showConfirmButton: true,
    });
  }


  // RECIVE FRIENDS REQUESTS

  messageReceived$: Subscription;
  friend_request: Friendship;

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

  private handleSocketMessage(message: SocketMessageGeneric<any>): void {
    switch (message.Type) {
      case SocketCommunicationType.REQUEST:
        console.log('Solicitud de amistad recibida:', message.Data);
        this.friend_request = message.Data;
        break;

      case SocketCommunicationType.CONNECTION:
        console.log('Mensaje de conexión recibido:', message.Data);

        const connectionModel = message.Data as ConnectionModel;

        if (connectionModel.Type == ConnectionType.Connected) {
          const friend = this.disconnectedFriends.find(friend => friend.id === connectionModel.UserId);

          if (friend) {
            // Eliminar de desconectados y agregar a conectados
            this.disconnectedFriends = this.disconnectedFriends.filter(f => f.id !== connectionModel.UserId);
            this.connectedFriends.push(friend);
          }
        } else {
          const friend = this.connectedFriends.find(friend => friend.id === connectionModel.UserId);

          if (friend) {
            // Eliminar de conectados y agregar a desconectados
            this.connectedFriends = this.connectedFriends.filter(f => f.id !== connectionModel.UserId);
            this.disconnectedFriends.push(friend);
          }
        }
        break;

      case SocketCommunicationType.GAME_INVITATION:
        const gameInvitation: GameInvitationModel = message.Data as GameInvitationModel;

        if (!gameInvitation) {
          console.error("Error: message.Data no es un GameInvitationModel válido", message.Data);
          break;
        }

        const friend = this.connectedFriends.find(friend => friend.id === gameInvitation.UserId);

        console.log("Invitation:", gameInvitation);


        // 🔹 Mostrar alerta para aceptar o rechazar
        Swal.fire({
          title: '<i class="fa-solid fa-chess-board"></i> ¡Invitación a partida!',
          text: 'Tu amigo ${friend?.userName} te ha invitado a jugar.',
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
            gameInvitation.State = FriendshipState.Accepted;
          } else {
            gameInvitation.State = FriendshipState.Canceled;
          }
        });
        
        
        this.gameInvitations.push(gameInvitation);
        break;


    }
  }


  // SEND FRIEND REQUEST

  async makeFriendshipRequest(id: number): Promise<Result<Friendship>> {
    const result = await this.api.post<Friendship>(`Friendship/makerequest?friendId=${id}`)
    if (!result.success) {
      this.handleError('No se pudo realizar la petición')
    }
    
    return result
  }


  async deleteFriend(friendId: number): Promise<void> {

    const result = await this.api.post(`User/deleteFriend?friendId=${friendId}`)

    const query: string = ""

    await this.getFriends(query)

  }

  async newGameInvitation(friendId: number): Promise<void> {
    console.log("New invitation")
    const result = await this.api.post(`User/newGameInvitation?friendId=${friendId}`)


  }


}

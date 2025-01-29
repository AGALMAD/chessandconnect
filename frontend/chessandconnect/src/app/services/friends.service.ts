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


@Injectable({
  providedIn: 'root'
})
export class FriendsService {
  

  constructor(private api: ApiService, private router: Router) { }
  constructor(
    private api: ApiService, 
    private router: Router,
    public webSocketService: WebsocketService
  ) {
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message => 
      await this.readMessage(message)
    );
   }


  async getUsersByNickname(userNickname: String): Promise<Result<Friend>> {
    const result = await this.api.get<Friend>('Friendship/getusers', userNickname)
    if (!result.success) {
      this.handleError('Usuario no encontrado');
    } 
    return result
  }

  async getFriends(): Promise<Result<Friend[]>> {
    const result = await this.api.get<Friend[]>('friendship/getfriends')
    if(!result.success) {
      this.handleError('No se encontraron amigos')
    }
    return result
  } 

  async acceptFriendshipRequest(id: number): Promise<Result<Friend>> {
    const result = await this.api.post<Friend>(`Friendship/acceptrequest?friendId=${id}`)
    if(result.success) {
      this.handleError('No se pudo aceptar la petición')
    }
    return result
  }

  async getAllFriendshipRequest(): Promise<Result<Friendship>> {
    const result = await this.api.get<Friendship>('friendship/getallrequests')
    if(result.success) {
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
    console.log('Mensaje recibido:', message);

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
    }
  }


  // SEND FRIEND REQUEST

  async makeFriendshipRequest(id: number): Promise<Result<Friendship>>{
    const result = await this.api.post<Friendship>(`Friendship/makerequest?friendId=${id}`)
    if(result.success) {
      this.handleError('No se pudo realizar la petición')
    }
    return result
  }

}

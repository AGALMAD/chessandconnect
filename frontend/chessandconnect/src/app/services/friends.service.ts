import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Router } from '@angular/router';
import { Friend } from '../models/dto/friend';
import { Result } from '../models/result';
import Swal from 'sweetalert2';
import { Friendship } from '../models/dto/friendship';

@Injectable({
  providedIn: 'root'
})
export class FriendsService {

  constructor(private api: ApiService, private router: Router) { }


  private async getUsersByNickname(userNickname: String): Promise<Result<Friend>> {
    const result = await this.api.get<Friend>('Friendship/getusers', userNickname)
    if (!result.success) {
      this.handleError('Usuario no encontrado');
    } 
    return result
  }

  private async getFriends(): Promise<Result<Friend>> {
    const result = await this.api.get<Friend>('friendship/getfriends')
    if(!result.success) {
      this.handleError('No se encontraron amigos')
    }
    return result
  } 

  private async makeFriendshipRequest(id: number): Promise<Result<Friendship>>{
    const result = await this.api.post<Friendship>(`Friendship/makerequest?friendId=${id}`)
    if(result.success) {
      this.handleError('No se pudo realizar la petición')
    }
    return result
  }

  private async acceptFriendshipRequest(id: number): Promise<Result<Friend>> {
    const result = await this.api.post<Friend>(`Friendship/acceptrequest?friendId=${id}`)
    if(result.success) {
      this.handleError('No se pudo aceptar la petición')
    }
    return result
  }

  private async getAllFriendshipRequest(): Promise<Result<Friendship>> {
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
}

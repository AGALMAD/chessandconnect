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
    const result = await this.api.get<Friend>('Friendship/user', userNickname)
    if (!result.success) {
      this.handleError('Usuario no encontrado');
    } 
    return result
  }

  /* private async requestFrienship(userNickname: String): Promise<Result<Friendship>{

  } */


  private handleError(message: string): void {
    Swal.fire({
      icon: 'error',
      text: message,
      showConfirmButton: true,
    });
}
}

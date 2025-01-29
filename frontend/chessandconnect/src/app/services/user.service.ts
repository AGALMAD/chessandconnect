import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { User } from '../models/dto/user';
import { Result } from '../models/result';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  currentUser: User

  constructor(private api: ApiService) {}

  async getUsersByNickname(userNickname: String): Promise<void> {
    const result = await this.api.get<User>('User', userNickname)
    if (!result.success) {
      this.api.handleError('Usuario no encontrado');
    }

    this.currentUser = result.data
  }

}

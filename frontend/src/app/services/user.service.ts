import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { User } from '../models/dto/user';
import { Result } from '../models/result';
import Swal from 'sweetalert2';
import { Register } from '../models/dto/register';
import { Pagination } from '../models/dto/pagination';
import { Play } from '../models/play';
import { GamesHistory } from '../models/game-history';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private api: ApiService) {}


  getSearchUsers(query: string){
    return this.api.get<User>(`User/searchUser?query=${query}`)
  }

  getUser(id: number){
    return this.api.get<User>(`User/getUserById?id=${id}`)
  }

  updateUser(authData: Register){
    this.api.post<Register>('User/updateUser', authData)
  }

  updatePassword(authData: Register){
    console.log(authData)
    return this.api.post<Register>('User/updateUserPassword', authData)
  }

  updateAvatar(image: File){
    const formData = new FormData();
    formData.append('ImagePath', image);
    this.api.post<File>('User/updateAvatar', formData)
  }

  async getGamesHistory(pagination: Pagination){
    return await this.api.post<GamesHistory>('User/gamesHistory', pagination)
  }


  public handleError(message: string): void {
    Swal.fire({
      icon: 'error',
      text: message,
      showConfirmButton: true,
    });
  }
}


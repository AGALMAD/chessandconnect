import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { User } from '../models/dto/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private api: ApiService
  ) { }

  getSearchUsers(query: string){
    return this.api.get<User>(`User/searchUser?query=${query}`)
  }

}

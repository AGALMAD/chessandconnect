import { Injectable } from '@angular/core';
import { Friend } from '../models/dto/friend';
import { User } from '../models/dto/user';

@Injectable({
  providedIn: 'root'
})
export class PlayService {

  opponent: User
  

  constructor() { }
}

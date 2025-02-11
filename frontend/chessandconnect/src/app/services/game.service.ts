import { Injectable } from '@angular/core';
import { Friend } from '../models/dto/friend';
import { User } from '../models/dto/user';
import { BaseBoard } from '../models/Games/Base/BaseBoard';

@Injectable({
  providedIn: 'root'
})
export class GameService {


  isHost = false
  opponent: User
  board: BaseBoard

  constructor() { }
}

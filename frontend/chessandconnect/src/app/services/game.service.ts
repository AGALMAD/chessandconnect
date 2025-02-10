import { Injectable } from '@angular/core';
import { Friend } from '../models/dto/friend';
import { User } from '../models/dto/user';

@Injectable({
  providedIn: 'root'
})
export class GameService {


  isHost = false
  opponent: User

  //Guardar el juego con la sala y el tablero

  constructor() { }
}

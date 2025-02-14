import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { ChessPiece } from '../models/Games/Chess/ChessPiece';

@Injectable({
  providedIn: 'root'
})
export class GameService{


  isHost = false
  opponent: User
  pieces: ChessPiece[]

  constructor() { }
}

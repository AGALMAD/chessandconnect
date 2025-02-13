import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { ChessBasePiece } from '../models/Games/Chess/ChessBasePiece';

@Injectable({
  providedIn: 'root'
})
export class GameService{


  isHost = false
  opponent: User
  pieces: ChessBasePiece[]

  constructor() { }
}

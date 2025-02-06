import { Injectable } from '@angular/core';
import { User } from '../models/dto/user';
import { Friend } from '../models/dto/friend';

@Injectable({
  providedIn: 'root'
})
export class MatchMakingService {


  isHost = true

  opponent: Friend


  constructor() { }
}

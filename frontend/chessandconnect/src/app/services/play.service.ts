import { Injectable } from '@angular/core';
import { Friend } from '../models/dto/friend';

@Injectable({
  providedIn: 'root'
})
export class PlayService {

  opponent: Friend
  

  constructor() { }
}

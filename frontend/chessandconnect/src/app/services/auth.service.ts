import { Injectable } from '@angular/core';
import { Login } from '../models/dto/login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  async login(data: Login){

  }
}

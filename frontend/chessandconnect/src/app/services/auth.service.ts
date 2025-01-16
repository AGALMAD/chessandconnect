import { Injectable } from '@angular/core';
import { Login } from '../models/dto/login';
import { AuthResponse } from '../models/dto/auth-response';
import { Register } from '../models/dto/register';
import { ApiService } from './api.service';
import { Router } from '@angular/router';
import { Result } from '../models/result';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private api:ApiService, private router:Router) { }

  async login(authLogin: Login): Promise<Result<AuthResponse>>{
    const result = await this.api.get<AuthResponse>('Auth/login',authLogin)

    return result
  }

  async register(authRegister: Register): Promise<Result<AuthResponse>> {
    const result = await this.api.post<AuthResponse>('Auth/Register', authRegister);

    /* if (result.success) {
      this.setSession(result.data.accessToken, true);
    } else {
      this.handleError('Ha habido un problema al registrar el usuario.');
    } */
    return result;
  }
}

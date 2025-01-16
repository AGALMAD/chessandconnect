import { Injectable } from '@angular/core';
import { Login } from '../models/dto/login';
import { AuthResponse } from '../models/dto/auth-response';
import { Register } from '../models/dto/register';
import { ApiService } from './api.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Result } from '../models/result';

import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'token';
  decodedToken: any = null;

  constructor(private api: ApiService, private router: Router) {
    this.loadTokenFromStorage();
  }

  private loadTokenFromStorage(): void {
    const token =
      sessionStorage.getItem(this.TOKEN_KEY) ||
      localStorage.getItem(this.TOKEN_KEY);
    if (token) {
      this.api.jwt = token;
      this.decodedToken = this.decodeJwt(token);
    }
  }

  private decodeJwt(token: string): any {
    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Error decodificando el token JWT:', error);
      return null;
    }
  }


  async login(authLogin: Login): Promise<Result<AuthResponse>> {
    const result = await this.api.post<AuthResponse>('Auth/login', authLogin)

    return result
  }

  async register(authRegister: Register): Promise<Result<AuthResponse>> {
    const result = await this.api.post<AuthResponse>('Auth/Register', authRegister);
    if (result.success) {
      this.setSession(result.data.accessToken, true);
    } else {
      this.handleError('Ha habido un problema al registrar el usuario.');
    }
    return result;
  }

  private setSession(token: string, remember: boolean): void {
    this.api.jwt = token;
    this.decodedToken = this.decodeJwt(token);
    if (remember) {
      localStorage.setItem(this.TOKEN_KEY, token);
    } else {
      sessionStorage.setItem(this.TOKEN_KEY, token);
    }
  }

  private handleError(message: string): void {
    Swal.fire({
      icon: 'error',
      text: 'Login Incorrecto',
      showConfirmButton: true,
    });
  }

  get isLoggedIn(): boolean {
    return !!this.decodedToken;
  }

  logout(): void {
    this.api.jwt = '';
    this.decodedToken = null;
    localStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
    this.router.navigate(['#']);
  }

  public handleSession(token: string, remember: boolean): void {
    this.clearSession();
    this.setSession(token, remember);
  }

  private clearSession(): void {
    this.api.jwt = null;
    this.decodedToken = null;
    localStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
  }
}


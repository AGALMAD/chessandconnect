import { Injectable } from '@angular/core';
import { Login } from '../models/dto/login';
import { AuthResponse } from '../models/dto/auth-response';
import { Register } from '../models/dto/register';
import { ApiService } from './api.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Result } from '../models/result';
import { User } from '../models/dto/user';

import Swal from 'sweetalert2';
import { WebsocketService } from './websocket.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public currentUser: User | null;

  private readonly TOKEN_KEY = 'token';
  decodedToken: any = null;

  constructor(
    private api: ApiService,
    private router: Router,
    private websocketService: WebsocketService,
    private userService: UserService
  ) {
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

  async login(
    authLogin: Login,
    remember: boolean
  ): Promise<Result<AuthResponse>> {
    const result = await this.api.post<AuthResponse>('Auth/login', authLogin);
    if (result.success) {
      await this.setSession(result.data.accessToken, remember);
    } else {
      this.handleError('Ha habido un problema al iniciar sesión.');
    }

    return result;
  }

  async register(
    authRegister: Register,
    remember: boolean
  ): Promise<Result<AuthResponse>> {
    //Paso a formData de los datos del usuario
    const formData = new FormData();
    formData.append('Username', authRegister.username);
    formData.append('Email', authRegister.email);
    formData.append('Password', authRegister.password);
    if (authRegister.image) {
      formData.append('ImagePath', authRegister.image);
    }

    const result = await this.api.post<AuthResponse>('Auth/register', formData);

    if (result.success) {
      await this.setSession(result.data.accessToken, remember);
    } else {
      this.handleError('Ha habido un problema al registrar el usuario.');
    }
    return result;
  }

  private async setSession(token: string, remember: boolean): Promise<void> {
    this.api.jwt = token;
    this.decodedToken = this.decodeJwt(token);
    if (remember) {
      localStorage.setItem(this.TOKEN_KEY, token);
    } else {
      sessionStorage.setItem(this.TOKEN_KEY, token);
    }
  }

  // Método para recuperar el token
  getToken(): string | null {
    if (
      localStorage.getItem('token') != '' &&
      localStorage.getItem('token') != null
    ) {
      return localStorage.getItem('token');
    } else if (
      sessionStorage.getItem('token') != '' &&
      sessionStorage.getItem('token') != null
    ) {
      return sessionStorage.getItem('token');
    }
    return null;
  }

  private handleError(message: string): void {
    Swal.fire({
      icon: 'error',
      text: 'Login Incorrecto',
      showConfirmButton: true,
    });
  }

  loged() {
    if (this.getToken()) {
      return true;
    }
    return false;
  }

  async logout(): Promise<void> {
    this.api.jwt = '';
    this.decodedToken = null;
    localStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
    this.router.navigate(['#']);

    await this.websocketService.disconnectRxjs();
  }

  public async handleSession(token: string, remember: boolean): Promise<void> {
    this.clearSession();
    await this.setSession(token, remember);
  }

  private clearSession(): void {
    this.api.jwt = null;
    this.decodedToken = null;
    localStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
  }

  getUser() {
    const token = this.decodeJwt(this.getToken());

    /*     const user: User = {
          id: token.id,
          userName: token.userName,
          email: token.email,
          avatarImageUrl: token.avatarImageUrl,
          plays: []
        } */
    return token;
  }

  async getCurrentUser(): Promise<void> {
    if (this.api.jwt && this.currentUser == null) {
      try {
        const result = await this.api.get<User>('User');
        this.currentUser = result.data;
        console.log('User: ', result.data);
        console.log('Current', this.currentUser);
      } catch (error) {
        console.error('Error obteniendo usuario:', error);
      }
    }
  }
}

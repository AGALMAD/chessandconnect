import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { UserDto } from '../models/dto/user-dto';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private api: ApiService) { }

  public async getUsers(): Promise<UserDto[]> {
    const result = await this.api.get<UserDto[]>('Admin/allusers');
    if (!result.success) {
      this.handleError('Error al obtener usuarios');
    } 
    return result.data; 
  }

  public async changeUserState(id:number): Promise<UserDto>{
    const result = await this.api.put<UserDto>(`Admin/${id}/editstatus`);
    if (!result.success) {
      this.handleError('Error al cambiar el estado del usuario');
    } 
    return result.data; 
  }

  public async changeUserRole(id:number): Promise<UserDto>{
    const result = await this.api.put<UserDto>(`Admin/${id}/editrole`);
    if (!result.success) {
      this.handleError('Error al cambiar el rol del usuario');
    } 
    return result.data; 
  }


  private handleError(message: string): void {
      Swal.fire({
        icon: 'error',
        text: message,
        showConfirmButton: true,
      });
    }

}

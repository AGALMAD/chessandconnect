import { Component, OnInit } from '@angular/core';
import { UserDto } from '../../models/dto/user-dto';
import { AdminService } from '../../services/admin.service';
import { Result } from '../../models/result';
import { NavbarComponent } from "../../components/navbar/navbar.component";
import { ApiService } from '../../services/api.service';
import { environment } from '../../../environments/environment';
import Swal from 'sweetalert2';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-admin',
  imports: [NavbarComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit {

  public baseUrl = environment.apiUrl; 
  
  users: UserDto[] = [];
  currentUserId: number;

  constructor(private api: ApiService, private adminService: AdminService, private authService: AuthService) { }

  ngOnInit() {
    this.currentUserId = this.authService.currentUser.id;
    this.getUsers(this.currentUserId);
  }

  
  async getUsers(id: number) {
    const result = await this.adminService.getUsers()
    const index = result.findIndex((user) => user.id === id);
    result.splice(index, 1);
    this.users = result
  }


  async changeState(id:number) {
    const result = await this.adminService.changeUserState(id);
    const index = this.users.findIndex((user) => user.id === id);
    this.users[index] = result;
    }
    
    async changeRole(id:number) {
    const result = await this.adminService.changeUserRole(id);
    const index = this.users.findIndex((user) => user.id === id);
    this.users[index] = result;
    }


async banUser(id: number, name: string) {
    //Mensaje de confirmación
    const result = await Swal.fire({
      title: `¿Estás seguro que quieres banear a ${name}?`,
      text: 'No podrás revertir esta acción.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, banear',
      cancelButtonText: 'Cancelar',
      customClass: {
        popup: 'bg-[#301e16] text-[#E8D5B5]',
        title: 'font-bold text-lg',
        confirmButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg',
        cancelButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg'
      }
    });
    console.log(result)
    if (result.isConfirmed) {
      console.log("usuario baneado: " + name);
      await this.changeState(id);
    } else {
      console.log("No se baneará al usuario" + name)
    }
  }

  async unbanUser(id: number, name: string) {
    //Mensaje de confirmación
    const result = await Swal.fire({
      title: `¿Estás seguro que quieres quitar el ban a ${name}?`,
      text: 'No podrás revertir esta acción.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, desbanear',
      cancelButtonText: 'Cancelar',
      customClass: {
        popup: 'bg-[#301e16] text-[#E8D5B5]',
        title: 'font-bold text-lg',
        confirmButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg',
        cancelButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg'
      }
    });
    if (result.isConfirmed) {
      console.log("usuario desbaneado: " + name);
      await this.changeState(id);
    } else {
      console.log("No se baneará al usuario" + name)
    }
  }

}

import { Component, OnInit } from '@angular/core';
import { UserDto } from '../../models/dto/user-dto';
import { AdminService } from '../../services/admin.service';
import { Result } from '../../models/result';
import { NavbarComponent } from "../../components/navbar/navbar.component";

@Component({
  selector: 'app-admin',
  imports: [NavbarComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit {

  
  users: UserDto[] = [];
  
  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.getUsers();
  }

  
  async getUsers() {
    const result = await this.adminService.getUsers()
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
}

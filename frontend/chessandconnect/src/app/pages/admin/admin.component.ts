import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  imports: [],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit {
unbanUser(arg0: any) {
throw new Error('Method not implemented.');
}
deleteUser(arg0: any) {
throw new Error('Method not implemented.');
}
changeUserRole(arg0: any,arg1: string) {
throw new Error('Method not implemented.');
}
  
  users: any[] = [];
  
  constructor() { }

  ngOnInit() {
    this.getUsers();
  }

  
  getUsers() {
    throw new Error('Method not implemented.');
  }

}

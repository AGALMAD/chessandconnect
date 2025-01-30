import { Component, Inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/dto/user';
import { Result } from '../../models/result';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FriendsService } from '../../services/friends.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


@Component({
  selector: 'app-user-list',
  imports: [FormsModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {

  users: User[] = []
  searchQuery: string;
  private searchTimeout: any;

  constructor(
    private userService: UserService,
    private router: Router,
    private friendService: FriendsService,
    public dialogRef: MatDialogRef<UserListComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ){}

  async onSearch(){
    clearTimeout(this.searchTimeout);
    this.searchTimeout = setTimeout(async () => {
        this.users = [];

        if (!this.searchQuery) {
            this.searchQuery = '';
        }

        const users: Result<User> = await this.userService.getSearchUsers(this.searchQuery);
        this.users = this.users.concat(users.data);;
    }, 500); // Espera 500ms antes de ejecutar
  }

  sendFriendRequest(destination_id: number){
    this.friendService.makeFriendshipRequest(destination_id)
  }

  goToProfile(id: number){
    this.router.navigate(
      ['/profile'],
      { queryParams: { 'id': id, } }
    );
  }

  closeModal() {
    this.dialogRef.close();
  }

}

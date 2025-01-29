import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { query } from '@angular/animations';
import { User } from '../../models/dto/user';
import { Result } from '../../models/result';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FriendsService } from '../../services/friends.service';
import { Friendship } from '../../models/dto/friendship';

@Component({
  selector: 'app-user-list',
  imports: [FormsModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {

  users: User[] = []
  searchQuery: string;

  constructor(
    private userService: UserService,
    private router: Router,
    private friendService: FriendsService
  ){}

  async onSearch(){
    this.users.splice(0, this.users.length);

    if(!this.searchQuery){
      this.searchQuery = ""
    }
    const users: Result<User> =  await this.userService.getSearchUsers(this.searchQuery)
    this.users = this.users.concat(users.data);
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

}

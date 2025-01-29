import { Component, OnInit } from '@angular/core';
import { FriendsService } from '../../services/friends.service';
import { Friend } from '../../models/dto/friend';
import { User } from '../../models/dto/user';

@Component({
  selector: 'app-friends-list',
  imports: [],
  templateUrl: './friends-list.component.html',
  styleUrl: './friends-list.component.css'
})
export class FriendsListComponent implements OnInit {

  connectedFriends : Friend[]
  disconnectedFriends : Friend[]

  friendRequest: Friend[]


  constructor(
    private friendService: FriendsService
  ) {
  }
  
  async ngOnInit(): Promise<void> {
    await this.getUserFriends();
  }




  async getUserFriends() {
    const result = await this.friendService.getFriends();
    this.friendList = result.data
  }
  

}

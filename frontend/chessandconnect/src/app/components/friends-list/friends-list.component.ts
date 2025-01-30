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
export class FriendsListComponent {

  connectedFriends : Friend[]
  disconnectedFriends : Friend[]

  friendRequest: Friend[]


  constructor(
    private friendService: FriendsService
  ) {}
  




}

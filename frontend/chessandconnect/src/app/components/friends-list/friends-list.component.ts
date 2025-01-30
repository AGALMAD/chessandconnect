import { Component, OnInit } from '@angular/core';
import { FriendsService } from '../../services/friends.service';
import { Friend } from '../../models/dto/friend';
import { User } from '../../models/dto/user';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-friends-list',
  imports: [RouterLink],
  templateUrl: './friends-list.component.html',
  styleUrl: './friends-list.component.css'
})
export class FriendsListComponent implements OnInit {


  constructor(
    public friendService: FriendsService) {}


  async ngOnInit(): Promise<void> {
    console.log("obtener amigos")
    await this.friendService.getFriends()
  }
  

  async deleteFriend(friendId: number){
    console.log("Eliminado" + friendId)
    await this.friendService.deleteFriend(friendId)
  }




}

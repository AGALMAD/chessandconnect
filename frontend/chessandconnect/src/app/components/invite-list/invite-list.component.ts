import { Component } from '@angular/core';
import { FriendsService } from '../../services/friends.service';

@Component({
  selector: 'app-invite-list',
  imports: [],
  templateUrl: './invite-list.component.html',
  styleUrl: './invite-list.component.css'
})
export class InviteListComponent {
  constructor(
    public friendService: FriendsService) { }

    async newGameInvitation(friendId: number){
      await this.friendService.newGameInvitation(friendId)
    }
}

import { Component, Inject } from '@angular/core';
import { FriendsService } from '../../services/friends.service';
import { GameType } from '../../enums/game';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-invite-list',
  imports: [],
  templateUrl: './invite-list.component.html',
  styleUrl: './invite-list.component.css'
})
export class InviteListComponent {

  game: GameType

  constructor(
    public friendService: FriendsService,
    @Inject(MAT_DIALOG_DATA) public data: any

  ) { 
    this.game = data.game
  }

  async newGameInvitation(friendId: number) {
    await this.friendService.newGameInvitation(friendId, this.game)
  }
}

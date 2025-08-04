import { Component, Inject } from '@angular/core';
import { FriendsService } from '../../services/friends.service';
import { Router } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Friend } from '../../models/dto/friend';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-game-invitation',
  imports: [],
  templateUrl: './game-invitation.component.html',
  styleUrl: './game-invitation.component.css'
})
export class GameInvitationComponent {

  public baseUrl = environment.apiUrl;


  friend: Friend

  constructor(
    public router: Router,
    public friendService: FriendsService,
    public dialogRef: MatDialogRef<Friend>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }



  getConnectedFriendById(friendId: number) {
    this.friend = this.friendService.getConnectedFriendById(friendId)
  }


  closeModal() {
    this.dialogRef.close();
  }

}

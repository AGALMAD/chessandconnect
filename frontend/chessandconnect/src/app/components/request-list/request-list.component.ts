import { Component, Inject, OnInit } from '@angular/core';
import { Friendship } from '../../models/dto/friendship';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { FriendsService } from '../../services/friends.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserListComponent } from '../user-list/user-list.component';
import { Result } from '../../models/result';
import { Friend } from '../../models/dto/friend';

@Component({
  selector: 'app-request-list',
  imports: [],
  templateUrl: './request-list.component.html',
  styleUrl: './request-list.component.css'
})
export class RequestListComponent implements OnInit{

  requestList: Friend[] = []


  constructor(
    private userService: UserService,
    private router: Router,
    private friendService: FriendsService,
    public dialogRef: MatDialogRef<UserListComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ){}
  ngOnInit(): void {
    this.getAllRequests()
  }



  async getAllRequests() {
    const requestList : Result<Request[]> = await this.friendService.getAllFriendshipRequest()
  }


  closeModal() {
    this.dialogRef.close();
  }

}

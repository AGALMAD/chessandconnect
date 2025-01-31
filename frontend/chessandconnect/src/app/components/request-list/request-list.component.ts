import { Component, Inject, OnInit } from '@angular/core';
import { Friendship } from '../../models/dto/friendship';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { FriendsService } from '../../services/friends.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserListComponent } from '../user-list/user-list.component';

import { Result } from '../../models/result';
import { Friend } from '../../models/dto/friend';
import { RequestFriendship } from '../../models/dto/request-friendship';


@Component({
  selector: 'app-request-list',
  imports: [],
  templateUrl: './request-list.component.html',
  styleUrl: './request-list.component.css'
})
export class RequestListComponent implements OnInit{


  requestList: RequestFriendship[] = []



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
    const result : Result<RequestFriendship[]> = await this.friendService.getAllFriendshipRequest()
    this.requestList = result.data
  }


  closeModal() {
    this.dialogRef.close();
  }

}

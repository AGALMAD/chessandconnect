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
import { environment } from '../../../environments/environment';


@Component({
  selector: 'app-request-list',
  imports: [],
  templateUrl: './request-list.component.html',
  styleUrl: './request-list.component.css'
})
export class RequestListComponent implements OnInit{

  public baseUrl = environment.apiUrl;

  requestList: RequestFriendship[] = []



  constructor(
    private userService: UserService,
    private router: Router,
    public friendService: FriendsService,
    public dialogRef: MatDialogRef<UserListComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ){}

  ngOnInit(): void {
    this.getAllRequests()
    
  }

  async acceptRequest(id: number) {
    this.friendService.acceptFriendshipRequest(id)
    this.requestList = this.requestList.filter(x => x.userId != id);
  }

  async rejectRequest(id: number) {
    this.friendService.rejectFriendshipRequest(id)
    this.requestList = this.requestList.filter(x => x.userId != id);
  }

  async getAllRequests() {
    const result : Result<RequestFriendship[]> = await this.friendService.getAllFriendshipRequest()
    this.requestList = result.data
  }


  closeModal() {
    this.dialogRef.close();
  }

}

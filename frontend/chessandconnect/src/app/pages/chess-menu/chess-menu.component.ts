import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FriendsListComponent } from '../../components/friends-list/friends-list.component';
import { MenuService } from '../../services/menu.service';
import { ApiService } from '../../services/api.service';
import { UserListComponent } from '../../components/user-list/user-list.component';
import { WebsocketService } from '../../services/websocket.service';
import { MatDialog } from '@angular/material/dialog';
import { NavigationStart, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { GameInvitationComponent } from '../../components/game-invitation/game-invitation.component';

@Component({
  selector: 'app-chess-menu',
  imports: [NavbarComponent, FriendsListComponent, UserListComponent],
  templateUrl: './chess-menu.component.html',
  styleUrl: './chess-menu.component.css'
})
export class ChessMenuComponent implements OnInit {


  constructor(
    public menuService: MenuService,
    private api: ApiService,
    private webSocketService: WebsocketService,

  ) {
    
  }


  async ngOnInit(): Promise<void> {
    await this.webSocketService.connectRxjs()
  }

  


}

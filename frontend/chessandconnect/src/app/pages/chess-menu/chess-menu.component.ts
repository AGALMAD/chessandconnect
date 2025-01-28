import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FriendsListComponent } from '../../components/friends-list/friends-list.component';
import { WebsocketService } from '../../services/websocket.service';
import { Subscription } from 'rxjs';
import { MenuService } from '../../services/menu.service';
import { UserListComponent } from '../../components/user-list/user-list.component';

@Component({
  selector: 'app-chess-menu',
  imports: [NavbarComponent, FriendsListComponent,UserListComponent],
  templateUrl: './chess-menu.component.html',
  styleUrl: './chess-menu.component.css'
})
  export class ChessMenuComponent {

  constructor(menuService : MenuService){}
}

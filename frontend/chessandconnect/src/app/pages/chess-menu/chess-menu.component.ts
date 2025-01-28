import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FriendsListComponent } from '../../components/friends-list/friends-list.component';
import { MenuService } from '../../services/menu.service';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-chess-menu',
  imports: [NavbarComponent, FriendsListComponent],
  templateUrl: './chess-menu.component.html',
  styleUrl: './chess-menu.component.css'
})
  export class ChessMenuComponent implements OnInit, OnDestroy{

  constructor(public menuService : MenuService, private api: ApiService){}

  ngOnInit(): void {
    this.menuService.webSocketService.connectRxjs()
  }

  ngOnDestroy(): void {
    this.menuService.webSocketService.disconnectRxjs()
  }


}

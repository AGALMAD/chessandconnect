import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FriendsListComponent } from '../../components/friends-list/friends-list.component';
import { MenuService } from '../../services/menu.service';
import { ApiService } from '../../services/api.service';
import { WebsocketService } from '../../services/websocket.service';

@Component({
  selector: 'app-connect4-menu',
  imports: [NavbarComponent, FriendsListComponent],
  templateUrl: './connect4-menu.component.html',
  styleUrl: './connect4-menu.component.css'
})
export class Connect4MenuComponent implements OnInit {

  constructor(public menuService : MenuService, private api: ApiService, private webSocketService: WebsocketService) { }

  async ngOnInit(): Promise<void> {
    await this.webSocketService.connectRxjs()
  }
}

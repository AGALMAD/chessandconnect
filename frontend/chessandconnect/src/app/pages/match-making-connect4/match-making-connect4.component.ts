import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { environment } from '../../../environments/environment';
import { MenuService } from '../../services/menu.service';
import { ApiService } from '../../services/api.service';
import { WebsocketService } from '../../services/websocket.service';
import { Router } from '@angular/router';
import { MatchMakingService } from '../../services/match-making.service';
import { AuthService } from '../../services/auth.service';
import { FriendsService } from '../../services/friends.service';
import { Game } from '../../models/game';

@Component({
  selector: 'app-match-making-connect4',
  imports: [NavbarComponent],
  templateUrl: './match-making-connect4.component.html',
  styleUrl: './match-making-connect4.component.css'
})
export class MatchMakingConnect4Component {


  public baseUrl = environment.apiUrl;

  constructor(
    public menuService: MenuService,
    private api: ApiService,
    private webSocketService: WebsocketService,
    private router: Router,
    public matchMakingService: MatchMakingService,
    public authService: AuthService,
    private friendsService: FriendsService
  ) {
  }

  openLoadMatchMaking() {
    var loadView = document.getElementById('loadView') as HTMLElement;
    var main = document.getElementById('main') as HTMLElement;

    loadView.classList.remove('hidden');
    loadView.classList.add('flex');

    main.classList.remove('flex');
    main.classList.add('hidden');
  }

  closeLoadMatchMaking() {
    var loadView = document.getElementById('loadView') as HTMLElement;
    var main = document.getElementById('main') as HTMLElement;

    loadView.classList.remove('flex');
    loadView.classList.add('hidden');

    main.classList.remove('hidden');
    main.classList.add('flex');
  }



  friendInvitation(friendId: number) {
    this.friendsService.newGameInvitation(friendId, Game.Connect4)

  }

}

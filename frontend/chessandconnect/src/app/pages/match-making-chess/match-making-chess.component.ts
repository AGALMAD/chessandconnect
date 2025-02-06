import { Component, Input } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { MenuService } from '../../services/menu.service';
import { WebsocketService } from '../../services/websocket.service';
import { MatchMakingService } from '../../services/match-making.service';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';
import { Game } from '../../models/game';
import { FriendsListComponent } from '../../components/friends-list/friends-list.component';
import { FriendsService } from '../../services/friends.service';

@Component({
  selector: 'app-match-making-chess',
  imports: [NavbarComponent],
  templateUrl: './match-making-chess.component.html',
  styleUrl: './match-making-chess.component.css',
})
export class MatchMakingChessComponent {
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

  async openLoadMatchMaking() {
    //Muestra la vista de carga
    var loadView = document.getElementById('loadView') as HTMLElement;
    var main = document.getElementById('main') as HTMLElement;

    loadView.classList.remove('hidden');
    loadView.classList.add('flex');

    main.classList.remove('flex');
    main.classList.add('hidden');


    //Añade el jugador a la cola
    const result = await this.api.post(`Friendship/queueGame`, Game.Chess)

  }

  closeLoadMatchMaking() {
    var loadView = document.getElementById('loadView') as HTMLElement;
    var main = document.getElementById('main') as HTMLElement;

    loadView.classList.remove('flex');
    loadView.classList.add('hidden');

    main.classList.remove('hidden');
    main.classList.add('flex');
  }



  friendInvitation(friendId: number){
    this.friendsService.newGameInvitation(friendId,Game.Chess)

  }



}

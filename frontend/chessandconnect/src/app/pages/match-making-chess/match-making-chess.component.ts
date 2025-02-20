import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { MenuService } from '../../services/menu.service';
import { WebsocketService } from '../../services/websocket.service';
import { MatchMakingService } from '../../services/match-making.service';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';
import { GameType } from '../../enums/game';
import { FriendsService } from '../../services/friends.service';

@Component({
  selector: 'app-match-making-chess',
  imports: [NavbarComponent],
  templateUrl: './match-making-chess.component.html',
  styleUrl: './match-making-chess.component.css'
})
export class MatchMakingChessComponent {

  public baseUrl = environment.apiUrl;
  private gamemode = GameType.Chess 


  constructor(
    public menuService: MenuService,
    private api: ApiService,
    private webSocketService: WebsocketService,
    private router: Router,
    public matchMakingService: MatchMakingService,
    public authService: AuthService,
    public friendsService : FriendsService
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
    const result = await this.api.post(`MatchMaking/queueGame`, this.gamemode)

  }

  async closeLoadMatchMaking() {
    var loadView = document.getElementById('loadView') as HTMLElement;
    var main = document.getElementById('main') as HTMLElement;

    loadView.classList.remove('flex');
    loadView.classList.add('hidden');

    main.classList.remove('hidden');
    main.classList.add('flex');

    //Elimina el jugador a la cola
    const result = await this.api.post(`MatchMaking/cancelQueue`, this.gamemode)
  }



  friendInvitation(friendId: number){
    this.friendsService.newGameInvitation(friendId, this.gamemode)

  }


  async startGameWithFriend(){
    await this.matchMakingService.startGameWithFriend(this.gamemode)
  }

  async startGameWithBot(){
    await this.api.post('MatchMaking/IAGame', this.gamemode)
  }




  
}

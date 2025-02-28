import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { GameType } from '../../enums/game';
import { MenuService } from '../../services/menu.service';
import { WebsocketService } from '../../services/websocket.service';
import { ApiService } from '../../services/api.service';
import { MatchMakingService } from '../../services/match-making.service';
import { AuthService } from '../../services/auth.service';
import { FriendsService } from '../../services/friends.service';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-match-making-connect4',
  imports: [NavbarComponent],
  templateUrl: './match-making-connect4.component.html',
  styleUrl: './match-making-connect4.component.css'
})
export class MatchMakingConnect4Component implements OnInit{


  public baseUrl = environment.apiUrl;
  private gamemode = GameType.Connect4 


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

  async ngOnInit(): Promise<void> {

    this.authService.getCurrentUser();
    await this.webSocketService.connectRxjs()

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


  openFriendInvitationModal(){
    this.menuService.openFriendInvitationModal(GameType.Connect4)
  }
}

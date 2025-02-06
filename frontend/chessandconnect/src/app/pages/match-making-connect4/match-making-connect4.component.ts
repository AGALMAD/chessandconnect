import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';

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

  async openLoadMatchMaking() {
    var loadView = document.getElementById('loadView') as HTMLElement;
    var main = document.getElementById('main') as HTMLElement;

    loadView.classList.remove('hidden');
    loadView.classList.add('flex');

    main.classList.remove('flex');
    main.classList.add('hidden');


    //Añade el jugador a la cola
    const result = await this.api.post(`Friendship/queueGame`, Game.Connect4)

  }

  async closeLoadMatchMaking() {
    var loadView = document.getElementById('loadView') as HTMLElement;
    var main = document.getElementById('main') as HTMLElement;

    loadView.classList.remove('flex');
    loadView.classList.add('hidden');

    main.classList.remove('hidden');
    main.classList.add('flex');

    //Elimina el jugador a la cola
    const result = await this.api.post(`Friendship/cancelQueue`, Game.Connect4)
  }



  friendInvitation(friendId: number) {
    this.friendsService.newGameInvitation(friendId, Game.Connect4)

  }


  startGameWithFriend(){

  }

  startGameWithBot(){

    this.router.navigate(['/chessGame']);


  }

}

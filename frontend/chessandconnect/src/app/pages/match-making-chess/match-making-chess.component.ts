import { Component, Input } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { MenuService } from '../../services/menu.service';
import { WebsocketService } from '../../services/websocket.service';
import { MatchMakingService } from '../../services/match-making.service';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-match-making-chess',
  imports: [NavbarComponent],
  templateUrl: './match-making-chess.component.html',
  styleUrl: './match-making-chess.component.css'
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
  ) {}

  
  openLoadMatchMaking(){

    var loadView = document.getElementById("loadView") as HTMLElement
    var main = document.getElementById("main") as HTMLElement


    loadView.classList.remove("hidden")
    loadView.classList.add("flex")

    main.classList.remove("flex")
    main.classList.add("hidden")

  }

  closeLoadMatchMaking(){

    var loadView = document.getElementById("loadView") as HTMLElement
    var main = document.getElementById("main") as HTMLElement

    loadView.classList.remove("flex")
    loadView.classList.add("hidden")

    main.classList.remove("hidden")
    main.classList.add("flex")
  }

  
}

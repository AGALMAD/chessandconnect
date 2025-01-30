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

  private navigationSubscription: Subscription;

  constructor(
    public menuService: MenuService,
    private api: ApiService,
    private webSocketService: WebsocketService,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.navigationSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.dialog.closeAll(); // Cierra todos los modales abiertos
      }
    });
  }


  async ngOnInit(): Promise<void> {
    await this.webSocketService.connectRxjs()
  }

  ngOnDestroy() {
    if (this.navigationSubscription) {
      this.navigationSubscription.unsubscribe();
    }
  }

  openSearchModal() {
    this.dialog.open(UserListComponent, {
      width: '400px',
      data: {}  // Puedes pasar datos si necesitas
    });
  }

  openGameInvitationModal() {
    this.dialog.open(GameInvitationComponent, {
      width: '400px',
      data: {}  // Puedes pasar datos si necesitas
    });
  }


}

import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { WebsocketService } from './websocket.service';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { ConnectionModel } from '../models/WebSocketMessages/ConnectionModel';
import { MatDialog } from '@angular/material/dialog';
import { UserListComponent } from '../components/user-list/user-list.component';
import { GameInvitationComponent } from '../components/game-invitation/game-invitation.component';
import { NavigationStart, Router } from '@angular/router';
import { RequestListComponent } from '../components/request-list/request-list.component';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  connected$: Subscription;
  isConnected: boolean = false;
  messageReceived$: Subscription;
  disconnected$: Subscription;

  usersLogged = 0;
  gamesInProgress = 0;

  private navigationSubscription: Subscription;


  constructor(public webSocketService: WebsocketService ,
    private dialog: MatDialog,
    private router: Router) 
  {

    this.connected$ = this.webSocketService.connected.subscribe(() => this.isConnected = true);

    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message => 
      await this.readMessage(message)
    );

    this.disconnected$ = this.webSocketService.disconnected.subscribe(() => this.isConnected = false);


    this.navigationSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.dialog.closeAll(); // Cierra todos los modales abiertos
      }
    });
  }


  private async readMessage(message: string): Promise<void> {
    console.log('Mensaje recibido:', message);

    try {
      // Paso del mensaje a objeto
      const parsedMessage = JSON.parse(message);

      const socketMessage = new SocketMessageGeneric<any>();
      socketMessage.Type = parsedMessage.Type as SocketCommunicationType;
      socketMessage.Data = parsedMessage.Data;


      this.handleSocketMessage(socketMessage);
    } catch (error) {
      console.error('Error al parsear el mensaje recibido:', error);
    }
  }

  private handleSocketMessage(message: SocketMessageGeneric<any>): void {
    switch (message.Type) {
      case SocketCommunicationType.CONNECTION:
        console.log('Mensaje de conexión recibido:', message.Data);
        this.usersLogged = message.Data.UsersCounter
        break;
    }
  }

  public openSearchModal() {
    this.dialog.open(UserListComponent, {
      width: '400px',
      data: {}  // Puedes pasar datos si necesitas
    });
  }

  public openGameInvitationModal() {
    this.dialog.open(GameInvitationComponent, {
      width: '400px',
      data: {}  // Puedes pasar datos si necesitas
    });
  }

  openRequestModal() {
    this.dialog.open(RequestListComponent, {
      width: '400px',
      data: {}  // Puedes pasar datos si necesitas
    });
  }
}



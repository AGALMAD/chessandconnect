import { Injectable } from '@angular/core';
import { WebsocketService } from './websocket.service';
import { AuthService } from './auth.service';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root'
})
export class ConnectService {

  constructor(
    public webSocketService: WebsocketService,
    private authService: AuthService,
    private dialog: MatDialog,
  ) {
    /*
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message =>
      await this.readMessage(message)
    );
    */
  }
}
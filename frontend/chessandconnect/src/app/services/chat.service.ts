import { Injectable, Type } from '@angular/core';
import { WebsocketService } from './websocket.service';
import { Subscription } from 'rxjs';
import { SocketCommunicationType } from '../enums/SocketCommunicationType';
import { Chat } from '../models/dto/chat';
import { MatchMakingService } from './match-making.service';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import { SocketMessageGeneric } from '../models/WebSocketMessages/SocketMessage';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  messageReceived$: Subscription;

  messages: Chat[] = []

  constructor(
    private api: ApiService,
    public webSocketService: WebsocketService,
    private matchMatchMakingService: MatchMakingService,
    private authService: AuthService

  ) {
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message =>
      await this.readMessage(message)
    );
  }

  public async SendMessage(m: string) {
    const chat: Chat = {
      Message: m
    }
    const message: SocketMessageGeneric<Chat> = {
      Type: SocketCommunicationType.CHAT,
      Data: chat
    }

    this.webSocketService.sendRxjs(JSON.stringify(message))

  }

  private async readMessage(message: string): Promise<void> {
    console.log('Chat1:', message);

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

  private async handleSocketMessage(message: SocketMessageGeneric<any>): Promise<void> {

    console.log("Chat2:", message)

    switch (message.Type) {
      case SocketCommunicationType.CHAT:

        this.messages.push(message.Data)
        console.log("Chat3", this.messages)

        break

    }

  }
}

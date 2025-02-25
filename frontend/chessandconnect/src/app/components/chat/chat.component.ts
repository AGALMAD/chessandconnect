import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../../services/chat.service';
import { AuthService } from '../../services/auth.service';
import { Chat } from '../../models/dto/chat';
import { User } from '../../models/dto/user';
import { User_Chat } from '../../models/dto/user-chat';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-chat',
  imports: [FormsModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent {

  message: string

  constructor(public chatService: ChatService, public authService: AuthService, public gameService: GameService) { }

  OnInit() {
    console.log(this.chatService.messages)
  }

  sendMessage() {
    const user_chat: User_Chat = {
      userId: this.authService.currentUser.id,
      Message: this.message
    }
    this.chatService.messages.push(user_chat)
    this.chatService.SendMessage(this.message)
  }
}

import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../../services/chat.service';
import { AuthService } from '../../services/auth.service';
import { Chat } from '../../models/dto/chat';
import { User } from '../../models/dto/user';

@Component({
  selector: 'app-chat',
  imports: [FormsModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent {

  message: String

  constructor(public chatService: ChatService, public authService: AuthService){}

  OnInit(){
    console.log(this.chatService.messages)
  }

  sendMessage(){
    const chat: Chat = {
      PlayerId: this.authService.currentUser.id,
      Message: this.message
    }
    this.chatService.SendMessage(chat)
  }
}

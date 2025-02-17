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

  message: string

  constructor(public chatService: ChatService, public authService: AuthService){}

  OnInit(){
    console.log(this.chatService.messages)
  }

  sendMessage(){
    this.chatService.SendMessage(this.message)
  }
}

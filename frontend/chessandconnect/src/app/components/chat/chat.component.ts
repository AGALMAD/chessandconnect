import { Component, OnDestroy } from '@angular/core';
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
export class ChatComponent implements OnDestroy {

  message: string

  isModalOpen = false;
  modalMessage = "";
  modalAction: string = "";

  constructor(public chatService: ChatService, public authService: AuthService, public gameService: GameService) { }

  ngOnDestroy(): void {
    this.chatService.messages = []
  }



  sendMessage() {
    if (!this.message.trim()) return; 

    const user_chat: User_Chat = {
        userId: this.authService.currentUser.id,
        Message: this.message
    };

    this.chatService.messages.push(user_chat);
    this.chatService.SendMessage(this.message);

    this.message = ""; 
}


  openModal(action: string) {
    this.modalAction = action;
    this.isModalOpen = true;
    
    if (action === "leave") {
      this.modalMessage = "¿Seguro que quieres rendirte?";
    } else if (action === "draw") {
      this.modalMessage = "¿Quieres proponer tablas?";
    }
  }

  closeModal() {
    this.isModalOpen = false;
  }

  confirmAction() {
    if (this.modalAction === "leave") {
      this.gameService.leaveGame();
    } else if (this.modalAction === "draw") {
      this.gameService.drawRequest();
    }
    this.isModalOpen = false;
  }
  
}

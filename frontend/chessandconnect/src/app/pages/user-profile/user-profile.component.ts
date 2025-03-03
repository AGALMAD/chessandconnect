import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { User } from '../../models/dto/user';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';
import { ActivatedRoute, ParamMap, Router, ROUTER_CONFIGURATION } from '@angular/router';
import { Subscription } from 'rxjs';
import { FriendsService } from '../../services/friends.service';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { Register } from '../../models/dto/register';
import { Play } from '../../models/play';
import { GameType } from '../../enums/game';
import { PipeTimerPipe } from "../../pipes/pipe-timer.pipe";
import { playState } from '../../enums/playState';
import { Pagination } from '../../models/dto/pagination';
import { UserListComponent } from '../../components/user-list/user-list.component';
import { WebsocketService } from '../../services/websocket.service';
import { GamesHistory } from '../../models/game-history';

@Component({
  selector: 'app-user-profile',
  imports: [NavbarComponent, CommonModule, FormsModule, PipeTimerPipe],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
activeTab: GameType = GameType.Chess;
public baseUrl = environment.apiUrl;

queryMap: ParamMap;
routeQueryMap$: Subscription;
GameType = GameType


profileId: number;
user: User;
isFriend: boolean;


editType: 'name' | 'email' | 'password' = 'name';
newValue: string = '';
newPassword: string = '';
confirmPassword: string = '';
passwordError: string = '';
isModalOpen = false;

games: GamesHistory

actualPage: number = 1;
pageSize: number = 5;
totalPages: number


constructor(
    public authService: AuthService,
    private userService: UserService,
    private webSocketService: WebsocketService,
    private router: Router,
    private route: ActivatedRoute,
    private friendService: FriendsService
) {}

async ngOnInit(): Promise<void>{
  const pagination = sessionStorage.getItem("pagination")
if(pagination){
  const jsonPagination: Pagination = JSON.parse(pagination)
  this.pageSize = jsonPagination.GamesCuantity
  this.activeTab = jsonPagination.GameType
}

  this.authService.getCurrentUser();
  this.routeQueryMap$ = this.route.queryParamMap.subscribe(queryMap => this.getQueryId(queryMap));
  await this.webSocketService.connectRxjs()
  console.log("gola")
  this.loadGames(this.activeTab)
}


goToProfile(id: number) {
  this.router.navigate(
    ['/profile'],
    { queryParams: { 'id': id, } }
  );
}

async getQueryId(queryMap: ParamMap) {
  console.log(queryMap)
  this.profileId = parseInt(queryMap.get('id'));
  this.user = (await this.userService.getUser(this.profileId)).data;
  this.checkFriendship();
}


async getPlayerName(id: number){

  const result = await this.userService.getUser(id)

  if(result.success){
    const name = (result.data.userName)
    return name
  }
  return ""
}

getResultGame(result: playState){
  switch(result){
    case(-1):
      return "Perdida"
    
    case(0):
      return "Empatada"

    case(1):
      return "Ganada"
  }

 return null
}

prevPage(gameType: GameType) {
  if (gameType === GameType.Chess && this.actualPage > 1) {
    this.actualPage--;
    this.loadGames(gameType);
  } else if (gameType === GameType.Connect4 && this.actualPage > 1) {
    this.actualPage--;
    this.loadGames(gameType);
  }
}

nextPage(gameType: GameType) {
  if (gameType === GameType.Chess) {
    this.actualPage++;
    this.loadGames(gameType);
  } else if (gameType === GameType.Connect4) {
    this.actualPage++;
    this.loadGames(gameType);
  }
}

goToFirstPage(gameType: GameType) {
  if (gameType === GameType.Chess) {
    this.actualPage = 1
    this.loadGames(gameType);
  } else if (gameType === GameType.Connect4) {
    this.actualPage = 1
    this.loadGames(gameType);
  }
}

goToLastPage(gameType: GameType) {
  if (gameType === GameType.Chess) {
    this.actualPage = this.totalPages
    this.loadGames(gameType);
  } else if (gameType === GameType.Connect4) {
    this.actualPage = this.totalPages
    this.loadGames(gameType);
  }
}

async loadGames(gameType: GameType) {

  const response = await this.userService.getGamesHistory(this.savePagination(gameType));
  console.log(response)

  if (response.success) {
    this.games = response.data;
    this.totalPages = response.data.totalPages 
  } else {
    this.games = null;
    this.totalPages = 0;
  }
  
}

changeTab(gameType: GameType) {
  this.activeTab = gameType;
  this.actualPage = 1;
  this.loadGames(gameType)
}

changePageSize(){
  this.loadGames(this.activeTab)

  setTimeout(() => {
    window.location.reload();
  }, 500);
}

savePagination(gameType: GameType){
  const pagination: Pagination = {
    UserId: this.profileId,
    GameType: gameType,
    GamesCuantity: this.pageSize,
    ActualPage: this.actualPage,
  }
console.log(pagination)
  sessionStorage.setItem("pagination", JSON.stringify(pagination))

  return pagination
}

async checkFriendship() {
  await this.friendService.getFriends("");
  
  this.isFriend = this.friendService.connectedFriends.some(friend => friend.id === this.profileId) ||
                  this.friendService.disconnectedFriends.some(friend => friend.id === this.profileId);
  console.log(this.isFriend);
}

openModal(type: 'name' | 'email' | 'password') {
  this.editType = type;
  if (type === 'name') {
    this.newValue = this.authService.currentUser.userName;
  } else if (type === 'email') {
    this.newValue = this.authService.currentUser.email;
  } else {
    this.newPassword = '';
    this.confirmPassword = '';
    this.passwordError = '';
  }
  this.isModalOpen = true;
}

closeModal() {
  this.isModalOpen = false;
}

saveChanges() {
  if (this.editType === 'name') {
    this.editName(this.newValue)

  } 
  
  else if (this.editType === 'email') {
    
    this.editEmail(this.newValue)
  } 
  
  else if (this.editType === 'password') {
    
    if (this.newPassword !== this.confirmPassword) {
      this.passwordError = 'Passwords do not match';
      return;
    }

    this.changePassword(this.newPassword);
  }

  this.closeModal();
}

editName(name: string) {
    const authData: Register = {
      username: name,
      email: this.authService.currentUser.email,
      password: "",
      image: null
    };

    this.userService.updateUser(authData)
    setTimeout(() => {
      window.location.reload();
    }, 500);

}

editEmail(email: string) {
  const authData: Register = {
    username: this.authService.currentUser.userName,
    email: email,
    password: "",
    image: null
  };

  this.userService.updateUser(authData)
  setTimeout(() => {
    window.location.reload();
  }, 500);

}

changePassword(password: string) {
  const authData: Register = {
    username: this.authService.currentUser.userName,
    email: this.authService.currentUser.email,
    password: password,
    image: null
  };
  const result = this.userService.updatePassword(authData)
  if(result){
    this.authService.logout()
  }
  setTimeout(() => {
    window.location.reload();
  }, 500);

}

changeAvatar(image: File) {
  console.log(image);
  this.userService.updateAvatar(image);
  
  setTimeout(() => {
    window.location.reload();
  }, 500);
}


deleteAvatar() {
  this.userService.updateAvatar(null)
  setTimeout(() => {
    window.location.reload();
  }, 500);

}

addFriend(id: number) {
  this.friendService.makeFriendshipRequest(id)
}

async removeFriend(id: number) {
      const result = await Swal.fire({
        title: '¿Estás seguro?',
        text: 'No podrás revertir esta acción.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        customClass: {
          popup: 'bg-[#301e16] text-[#E8D5B5]',
          title: 'font-bold text-lg',
          confirmButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg',
          cancelButton: 'bg-[#CBA77B] hover:bg-[#A68556] text-[#301e16] font-medium py-2 px-4 rounded-lg'
        }
      });
  
      if (result.isConfirmed) {
        console.log("Amigo eliminado: " + id);
        await this.friendService.deleteFriend(id);
      } else {
        console.log("Eliminación cancelada")
      }
}

onFileSelected(event: any) {
  const image = event.target.files[0] as File;
  this.changeAvatar(image)
}

}
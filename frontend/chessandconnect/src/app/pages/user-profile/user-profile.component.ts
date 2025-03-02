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

@Component({
  selector: 'app-user-profile',
  imports: [NavbarComponent, CommonModule, FormsModule, PipeTimerPipe],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
  activeTab: string = 'chess';
public baseUrl = environment.apiUrl;

queryMap: ParamMap;
routeQueryMap$: Subscription;
profileId: number;
user: User;
isFriend: boolean;
editType: 'name' | 'email' | 'password' = 'name';
newValue: string = '';
newPassword: string = '';
confirmPassword: string = '';
passwordError: string = '';
isModalOpen = false;
chessGames: Play[]
connect4Games: Play[]


constructor(
    public authService: AuthService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
    private friendService: FriendsService
) {}

ngOnInit() {
  console.log(this.authService.currentUser.plays)
  this.routeQueryMap$ = this.route.queryParamMap.subscribe(queryMap => this.getQueryId(queryMap));
}

async getQueryId(queryMap: ParamMap) {
  this.profileId = parseInt(queryMap.get('id'));
  this.user = (await this.userService.getUser(this.profileId)).data;
  this.checkFriendship();
}

showGames(){
  this.authService.currentUser.plays.forEach(game => {
    if(game.game == GameType.Chess){
      this.chessGames.push(game)
    }else{
      this.connect4Games.push(game)
    }
  });
}

getTimeDifference(startDate: Date, endDate: Date): number {
  if (!startDate || !endDate) return 0;
  return endDate.getTime() - startDate.getTime();
}

async getPlayerName(id: number){
  return (await this.userService.getUser(this.profileId)).data.userName
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


changeTab(tab: string) {
  this.activeTab = tab;
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
}

editEmail(email: string) {
  const authData: Register = {
    username: this.authService.currentUser.userName,
    email: email,
    password: "",
    image: null
  };

  this.userService.updateUser(authData)
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
}

changeAvatar(image: File) {
  console.log(image)
  this.userService.updateAvatar(image)
}

deleteAvatar() {
  this.userService.updateAvatar(null)
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
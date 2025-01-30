import { Component, OnInit } from '@angular/core';
import { FriendsService } from '../../services/friends.service';
import { Friend } from '../../models/dto/friend';
import { User } from '../../models/dto/user';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-friends-list',
  imports: [RouterLink],
  templateUrl: './friends-list.component.html',
  styleUrl: './friends-list.component.css'
})
export class FriendsListComponent implements OnInit {


  constructor(
    public friendService: FriendsService) { }


  async ngOnInit(): Promise<void> {
    console.log("obtener amigos")
    await this.friendService.getFriends()
  }



  async deleteFriend(friendId: number) {
    //Mensaje de confirmación
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
      console.log("Amigo eliminado: " + friendId);
      await this.friendService.deleteFriend(friendId);
    } else {
      console.log("Eliminación cancelada");
    }
  }


  async newGameInvitation(friendId: number){
    await this.friendService.newGameInvitation(friendId)
  }


  showInvitations(){
    console.log("Invitaciones : ", this.friendService.gameInvitations)
  }



}

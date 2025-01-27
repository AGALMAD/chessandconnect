import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FriendsListComponent } from '../../components/friends-list/friends-list.component';

@Component({
  selector: 'app-connect4-menu',
  imports: [NavbarComponent, FriendsListComponent],
  templateUrl: './connect4-menu.component.html',
  styleUrl: './connect4-menu.component.css'
})
export class Connect4MenuComponent {

}

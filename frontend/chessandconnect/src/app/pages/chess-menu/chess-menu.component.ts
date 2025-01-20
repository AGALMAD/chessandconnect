import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FriendsListComponent } from '../../components/friends-list/friends-list.component';

@Component({
  selector: 'app-chess-menu',
  imports: [NavbarComponent, FriendsListComponent],
  templateUrl: './chess-menu.component.html',
  styleUrl: './chess-menu.component.css'
})
export class ChessMenuComponent implements OnInit{

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  



}

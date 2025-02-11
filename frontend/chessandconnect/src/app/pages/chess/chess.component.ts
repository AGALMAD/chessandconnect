import { Component, OnInit } from '@angular/core';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-chess',
  imports: [],
  templateUrl: './chess.component.html',
  styleUrl: './chess.component.css'
})
export class ChessComponent implements OnInit{

  constructor(public gameService: GameService){}

  ngOnInit(): void {
    console.log("GAMEEEEEE:", this.gameService.pieces)
  }

}

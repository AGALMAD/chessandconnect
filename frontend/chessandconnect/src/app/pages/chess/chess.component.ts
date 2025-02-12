import { Component, OnInit } from '@angular/core';
import { GameService } from '../../services/game.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-chess',
  imports: [CommonModule],
  templateUrl: './chess.component.html',
  styleUrl: './chess.component.css'
})

export class ChessComponent implements OnInit{

  constructor(public gameService: GameService){}

  ngOnInit(): void {
    console.log("GAMEEEEEE:", this.gameService.pieces)
  }
  
  letters: string[] = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];
  rows: number[] = [8, 7, 6, 5, 4, 3, 2, 1];
  cols: number [] = [0, 1, 2, 3, 4, 5, 6, 7]
  cells: string[] = [];

  rowsReverse: number[] = [1, 2, 3, 4, 5, 6, 7, 8];
  lettersReverse: string[] = ['H', 'G', 'F', 'E', 'D', 'C', 'B', 'A'];

}
